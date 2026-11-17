import React, { useState, useEffect, useCallback, useMemo, useRef } from 'react';
import { Button, Card, CardBody, Row, Col, Badge } from 'reactstrap';
import Select from 'react-select';
import { Translate, withLocalize } from 'react-localize-redux';
import swal from 'sweetalert';
import { BookOpen, Plus, Pencil, Trash2, Search, Image as ImageIcon, Target, Languages } from 'lucide-react';
import { getAllStories, deleteStory, getStoryLevels, getStoryCategories } from '../../../../api/reading-api';
import Paginationn from '../../../shared-module-components/datatables/pagination';
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import '../style.css';

const StoryList = ({ translate, refreshKey, onCreate, onEdit }) => {
    const [stories, setStories] = useState([]);
    const [levels, setLevels] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(false);
    const limiter = 12

    const [search, setSearch] = useState('');
    const [appliedSearch, setAppliedSearch] = useState('');

    const [levelFilter, setLevelFilter] = useState(null);
    const [categoryFilter, setCategoryFilter] = useState(null);

    const [page, setPage] = useState(0);
    const [totalPages, setTotalPages] = useState(0);

    const categoryKey = useCallback(
        (id) => (categories.find((category) => category.Id === id) || {}).Name || '',
        [categories]
    );

    const levelOptions = useMemo(
        () => levels.map((level) => ({ 
            value: level.Id, 
            label: String(level.Id) 
        })),
        [levels]
    );

    const categoryOptions = useMemo(
        () => categories.map((category) => ({
            value: category.Id,
            label: translate(`dashboards.reading-and-content.categories.${category.Name}`)
        })),
        [categories, translate]
    );

    // Load a page of stories, applying the active filters.
    const loadStories = useCallback(async (pageIndex) => {
        setLoading(true);
        try {
            const res = await getAllStories(pageIndex, limiter, {
                levelId: levelFilter ? levelFilter.value : null,
                categoryId: categoryFilter ? categoryFilter.value : null,
                search: appliedSearch.trim() || null
            });
            const data = res.data || {};
            setStories(data.Items || []);
            setTotalPages(data.TotalPages || 0);
        } catch (error) {
            console.error('Error loading stories:', error);
            swal(translate('story-creator.error'), translate('story-creator.load-error'), 'error');
        } finally {
            setLoading(false);
        }
    }, [levelFilter, categoryFilter, appliedSearch]);

    // Load the levels and categories for the filters.
    const loadLevelsAndCategories = useCallback(async () => {
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
    }, []);

    useEffect(() => {
        loadLevelsAndCategories();
    }, [loadLevelsAndCategories]);

    useEffect(() => {
        loadStories(page);
    }, [page, loadStories]);

    const firstReload = useRef(true);

    useEffect(() => {
        if (firstReload.current) {
            firstReload.current = false;
            return;
        }
        loadStories(page);
    }, [refreshKey]);

    // Submit the current search
    const searchStory = useCallback(() => {
        setAppliedSearch(search.trim());
        setPage(0);
    }, [search]);

    // Change the filter and reset to the first page
    const changeFilter = (setter) => (value) => {
        setter(value);
        setPage(0);
    };

    // Confirm and delete a story
    const handleDelete = (story) => {
        swal({
            title: translate('story-creator.confirm-delete-title'),
            text:
                (translate('story-creator.confirm-delete-text')) + ` "${story.TitleEn || story.TitleEs}".`,
            icon: 'warning',
            buttons: [
                translate('story-creator.cancel'),
                translate('story-creator.delete')
            ],
            dangerMode: true
        }).then(async (confirmed) => {
            if (!confirmed) return;
            try {
                await deleteStory(story.Id);
                if (stories.length === 1 && page > 0) {
                    setPage((prev) => prev - 1);
                } else {
                    loadStories(page);
                }
                swal(translate('story-creator.deleted'), translate('story-creator.deleted-msg'), 'success');
            } catch (error) {
                console.error('Error deleting story:', error);
                swal(translate('story-creator.error'), translate('story-creator.delete-error'), 'error');
            }
        });
    };

    return (
        <div className="stories animate__animated animate__fadeIn">
            <div className="sc-header">
                <div className="sc-header-title">
                    <span className="sc-header-icon">
                        <BookOpen size={22} />
                    </span>
                    <div>
                        <h3>
                            <Translate id="story-creator.title"/>
                        </h3>
                        <p className="sc-subtitle">
                            <Translate id="story-creator.subtitle" />
                        </p>
                    </div>
                </div>
                <Button color="primary" className="sc-btn-primary" onClick={onCreate}>
                    <Plus size={18} />
                    <Translate id="story-creator.new"/>
                </Button>
            </div>

            <div className="sc-toolbar">
                <div className="sc-search">
                    <Search size={18} className="sc-search-icon" />
                    <input
                        type="text"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') searchStory();
                        }}
                        placeholder={translate('story-creator.search-placeholder')}
                    />
                </div>
                <Button color="primary" className="sc-btn-search" onClick={searchStory}>
                    <Search size={16} />
                    <Translate id="story-creator.search" />
                </Button>
                <div className="sc-filter">
                    <Select
                        classNamePrefix="sc-select"
                        isClearable
                        placeholder={translate('story-creator.filter-level')}
                        value={levelFilter}
                        onChange={changeFilter(setLevelFilter)}
                        options={levelOptions}
                    />
                </div>
                <div className="sc-filter">
                    <Select
                        classNamePrefix="sc-select"
                        isClearable
                        placeholder={translate('story-creator.filter-category')}
                        value={categoryFilter}
                        onChange={changeFilter(setCategoryFilter)}
                        options={categoryOptions}
                    />
                </div>
            </div>

            {loading ? (
                <LoadingFullWindow />
            ) : stories.length === 0 ? (
                <Card className="sc-empty">
                    <CardBody>
                        <BookOpen size={40} className="sc-empty-icon" />
                        <p>
                            <Translate id="story-creator.empty" />
                        </p>
                        <Button color="primary" outline onClick={onCreate}>
                            <Plus size={16} />
                            <Translate id="story-creator.create-first"/>
                        </Button>
                    </CardBody>
                </Card>
            ) : (
                <Row className="sc-grid">
                    {stories.map((story) => (
                        <Col key={story.Id} xl="4" lg="6" md="6" xs="12" className="mb-4">
                            <Card className="sc-card">
                                <div className="sc-card-cover">
                                    {story.ImageUrl ? (
                                        <img src={story.ImageUrl} alt={story.TitleEn} />
                                    ) : (
                                        <div className="sc-card-cover-placeholder">
                                            <ImageIcon size={32} />
                                        </div>
                                    )}
                                    <div className="sc-card-level-group">
                                        <Badge className="sc-card-level-num">
                                            <Translate id="story-creator.level" /> {story.LevelId}
                                        </Badge>
                                    </div>
                                </div>
                                <CardBody className="sc-card-body">
                                    <h5 className="sc-card-title">{story.TitleEn}</h5>
                                    <p className="sc-card-subtitle">{story.TitleEs}</p>

                                    <div className="sc-card-meta">
                                        <span className="sc-chip">
                                            {categoryKey(story.CategoryId) && (
                                                <Translate id={`dashboards.reading-and-content.categories.${categoryKey(story.CategoryId)}`} />
                                            )}
                                        </span>
                                    </div>

                                    <div className="sc-card-stats">
                                        <span>
                                            {(story.Slides || []).length}{' '}
                                            <Translate id="story-creator.slides"/>
                                        </span>
                                        <span>
                                            <Languages size={13} /> {story.WordCountEn}/{story.WordCountEs}{' '}
                                            <Translate id="story-creator.words"/>
                                        </span>
                                    </div>

                                    <div className="sc-card-actions">
                                        <Button
                                            size="sm"
                                            color="light"
                                            className="sc-btn-edit"
                                            onClick={() => onEdit(story.Id)}
                                        >
                                            <Pencil size={15} />
                                            <Translate id="story-creator.edit"/>
                                        </Button>
                                        <Button
                                            size="sm"
                                            color="light"
                                            className="sc-btn-delete"
                                            onClick={() => handleDelete(story)}
                                        >
                                            <Trash2 size={15} />
                                            <Translate id="story-creator.delete"/>
                                        </Button>
                                    </div>
                                </CardBody>
                            </Card>
                        </Col>
                    ))}
                </Row>
            )}

            {!loading && totalPages > 1 && (
                <div className="sc-pagination">
                    <Paginationn
                        dapper
                        currentPage={page}
                        totalPages={totalPages}
                        updateParent={setPage}
                    />
                </div>
            )}
        </div>
    );
};

export default withLocalize(StoryList);
