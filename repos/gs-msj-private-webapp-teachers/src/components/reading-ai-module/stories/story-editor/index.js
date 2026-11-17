import React, { useState, useEffect, useCallback } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col, Input, Label, FormGroup } from 'reactstrap';
import { PulseLoader } from 'react-spinners';
import Select from 'react-select';
import { Translate, withLocalize } from 'react-localize-redux';
import swal from 'sweetalert';
import { Plus, GripVertical, ChevronUp, ChevronDown, X, Save } from 'lucide-react';
import {getStoryById, createStory, updateStory, getStoryLevels, getStoryCategories} from '../../../../api/reading-api';
import { emptySlide, emptyForm, countWords } from '../helpers';
import '../style.css';

// Story editor modal for creating or editing a story
const StoryEditor = ({ translate, isOpen, storyId, onClose, onSaved }) => {
    const isEditing = storyId != null;

    const [levels, setLevels] = useState([]);
    const [categories, setCategories] = useState([]);
    const [form, setForm] = useState(emptyForm());
    const [errors, setErrors] = useState({});
    const [saving, setSaving] = useState(false);
    const [loadingStory, setLoadingStory] = useState(isEditing);

    const levelOptions = levels.map((level) => ({ 
        value: level.Id, 
        label: String(level.Id) 
    }));

    const categoryOptions = categories.map((category) => ({
        value: category.Id, 
        label: translate(`dashboards.reading-and-content.categories.${category.Name}`)
    }));

    // Load the levels and categories
    useEffect(() => {
        const loadLevelsAndCategories = async () => {
            try {
                const [levelsRes, categoriesRes] = await Promise.all([
                    getStoryLevels(),
                    getStoryCategories()
                ]);
                setLevels(levelsRes.data || []);
                setCategories(categoriesRes.data || []);
            } catch (error) {
                console.error('Error loading levels and categories:', error);
            }
        };
        loadLevelsAndCategories();
    }, []);

    // When editing, load the story into the form.
    useEffect(() => {
        if (!isEditing) return;
        const loadStory = async () => {
            setLoadingStory(true);
            try {
                const loadedForm = await getStoryById(storyId);
                setForm(loadedForm);
            } catch (error) {
                console.error('Error loading story:', error);
                onClose();
            } finally {
                setLoadingStory(false);
            }
        };
        loadStory();
    }, [storyId, isEditing, translate, onClose]);

    const closeModal = () => {
        if (saving || loadingStory) return;
        onClose();
    };

    const setField = (field, value) => {
        setForm((prev) => ({ ...prev, [field]: value }));
        setErrors((prev) => ({ ...prev, [field]: undefined }));
    };

    const setSlideField = (index, field, value) => {
        setForm((prev) => {
            const slides = prev.slides.map((slide, i) =>
                i === index ? { ...slide, [field]: value } : slide
            );
            return { ...prev, slides };
        });
    };

    // Add a new empty slide at the end of the slides array
    const addSlide = () => {
        setForm((prev) => ({
            ...prev,
            slides: [...prev.slides, emptySlide(prev.slides.length + 1)]
        }));
    };

    // Remove a slide by index
    const removeSlide = (index) => {
        setForm((prev) => {
            if (prev.slides.length === 1) return prev;
            const slides = prev.slides
                .filter((_, i) => i !== index)
                .map((slide, i) => ({ ...slide, slideOrder: i + 1 }));
            return { ...prev, slides };
        });
    };

    // Move a slide up or down in the slides array
    const moveSlide = (index, direction) => {
        setForm((prev) => {
            const target = index + direction;

            if (target < 0 || target >= prev.slides.length) return prev;
            const slides = prev.slides.slice();

            // Swap the slides at index and target
            [slides[index], slides[target]] = [slides[target], slides[index]];

            return {
                ...prev,
                slides: slides.map((slide, i) => ({ ...slide, slideOrder: i + 1 }))
            };
        });
    };

    // Frontend validation before saving
    const validate = () => {
        const next = {};
        if (!form.levelId) next.levelId = true;
        if (!form.categoryId) next.categoryId = true;
        if (!form.titleEn.trim()) next.titleEn = true;
        if (!form.titleEs.trim()) next.titleEs = true;
        if (!(Number(form.fluencyGoal) > 0)) next.fluencyGoal = true;
        if (!(Number(form.accuracyGoal) > 0)) next.accuracyGoal = true;
        if (!(Number(form.completenessGoal) > 0)) next.completenessGoal = true;
        if (!(Number(form.pronunciationGoal) > 0)) next.pronunciationGoal = true;
        if (form.slides.length === 0) next.slides = true;
        setErrors(next);
        return Object.keys(next).length === 0;
    };

    const handleSave = useCallback(async () => {
        if (!validate()) return;
        setSaving(true);

        try {
            if (isEditing) {
                await updateStory(form.id, form);
            } else {
                await createStory(form);
            }
        } catch (error) {
            console.error('Error saving story:', error);
            setSaving(false);
            return;
        }

        swal(translate('story-creator.saved'), isEditing ? translate('story-creator.updated-msg') : translate('story-creator.created-msg'), 'success');
        onSaved();
    }, [form, isEditing, translate, onSaved]);

    return (
        <Modal isOpen={isOpen} toggle={closeModal} size="lg" className="sc-modal" scrollable>
            <ModalHeader toggle={closeModal}>
                {isEditing ? (
                    <Translate id="story-creator.edit-story"/>
                ) : (
                    <Translate id="story-creator.new-story"/>
                )}
            </ModalHeader>
            <ModalBody>
                {loadingStory ? (
                    <div className="sc-loading">
                        <PulseLoader size={10} color="#395a8f" />
                    </div>
                ) : (
                    <>
                        <div className="sc-section">
                            <h6 className="sc-section-title">
                                <Translate id="story-creator.details"/>
                            </h6>
                            <Row>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.title-en"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            value={form.titleEn}
                                            invalid={!!errors.titleEn}
                                            onChange={(e) => setField('titleEn', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.title-es"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            value={form.titleEs}
                                            invalid={!!errors.titleEs}
                                            onChange={(e) => setField('titleEs', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="4">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.level"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Select
                                            classNamePrefix="sc-select"
                                            className={errors.levelId ? 'sc-select-error' : ''}
                                            placeholder={translate('story-creator.select-level')}
                                            value={levelOptions.find((o) => o.value === form.levelId) || null}
                                            onChange={(opt) => setField('levelId', opt ? opt.value : null)}
                                            options={levelOptions}
                                        />
                                    </FormGroup>
                                </Col>
                                <Col md="4">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.category"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Select
                                            classNamePrefix="sc-select"
                                            className={errors.categoryId ? 'sc-select-error' : ''}
                                            placeholder={translate('story-creator.select-category')}
                                            value={categoryOptions.find((option) => option.value === form.categoryId) || null}
                                            onChange={(opt) => setField('categoryId', opt ? opt.value : null)}
                                            options={categoryOptions}
                                        />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.fluency-goal"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            type="number"
                                            min="1"
                                            value={form.fluencyGoal}
                                            invalid={!!errors.fluencyGoal}
                                            onChange={(e) => setField('fluencyGoal', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.accuracy-goal"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            type="number"
                                            min="1"
                                            value={form.accuracyGoal}
                                            invalid={!!errors.accuracyGoal}
                                            onChange={(e) => setField('accuracyGoal', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.completeness-goal"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            type="number"
                                            min="1"
                                            value={form.completenessGoal}
                                            invalid={!!errors.completenessGoal}
                                            onChange={(e) => setField('completenessGoal', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                                <Col md="6">
                                    <FormGroup>
                                        <Label>
                                            <Translate id="story-creator.pronunciation-goal"/>
                                            <span className="sc-required">*</span>
                                        </Label>
                                        <Input
                                            type="number"
                                            min="1"
                                            value={form.pronunciationGoal}
                                            invalid={!!errors.pronunciationGoal}
                                            onChange={(e) => setField('pronunciationGoal', e.target.value)}
                                        />
                                    </FormGroup>
                                </Col>
                            </Row>
                        </div>

                        <div className="sc-section">
                            <div className="sc-section-header">
                                <h6 className="sc-section-title">
                                    <Translate id="story-creator.slides-title"/>
                                    <span className="sc-count-badge">{form.slides.length}</span>
                                </h6>
                                <div className="sc-word-preview">
                                    EN {countWords(form.slides, 'textEn')} · ES{' '} {countWords(form.slides, 'textEs')}
                                </div>
                            </div>

                            {form.slides.map((slide, index) => (
                                <div className="sc-slide" key={index}>
                                    <div className="sc-slide-head">
                                        <span className="sc-slide-order">
                                            <GripVertical size={15} />
                                            {index + 1}
                                        </span>
                                        <div className="sc-slide-tools">
                                            <button
                                                type="button"
                                                className="sc-icon-btn"
                                                disabled={index === 0}
                                                onClick={() => moveSlide(index, -1)}
                                                title={translate('story-creator.move-up')}
                                            >
                                                <ChevronUp size={16} />
                                            </button>
                                            <button
                                                type="button"
                                                className="sc-icon-btn"
                                                disabled={index === form.slides.length - 1}
                                                onClick={() => moveSlide(index, 1)}
                                                title={translate('story-creator.move-down')}
                                            >
                                                <ChevronDown size={16} />
                                            </button>
                                            <button
                                                type="button"
                                                className="sc-icon-btn sc-icon-btn-danger"
                                                disabled={form.slides.length === 1}
                                                onClick={() => removeSlide(index)}
                                                title={translate('story-creator.remove-slide')}
                                            >
                                                <X size={16} />
                                            </button>
                                        </div>
                                    </div>
                                    <Row>
                                        <Col md="6">
                                            <Label className="sc-slide-label">
                                                <Translate id="story-creator.text-en"/>
                                            </Label>
                                            <Input
                                                type="textarea"
                                                rows="3"
                                                value={slide.textEn}
                                                onChange={(e) => setSlideField(index, 'textEn', e.target.value)}
                                            />
                                        </Col>
                                        <Col md="6">
                                            <Label className="sc-slide-label">
                                                <Translate id="story-creator.text-es"/>
                                            </Label>
                                            <Input
                                                type="textarea"
                                                rows="3"
                                                value={slide.textEs}
                                                onChange={(e) => setSlideField(index, 'textEs', e.target.value)}
                                            />
                                        </Col>
                                    </Row>
                                </div>
                            ))}

                            <Button color="link" className="sc-add-slide" onClick={addSlide}>
                                <Plus size={16} />
                                <Translate id="story-creator.add-slide"/>
                            </Button>
                        </div>
                    </>
                )}
            </ModalBody>
            <ModalFooter>
                <button className="button-secondary-custom btn-default" onClick={closeModal} disabled={saving}>
                    <Translate id="story-creator.cancel"/>
                </button>
                <button
                    className="button-primary-custom btn-default"
                    onClick={handleSave}
                    disabled={saving || loadingStory}
                >
                    {saving ? <PulseLoader size={6} color="#fff" /> : <Save size={16} />}
                    <Translate id="story-creator.save"/>
                </button>
            </ModalFooter>
        </Modal>
    );
};

export default withLocalize(StoryEditor);
