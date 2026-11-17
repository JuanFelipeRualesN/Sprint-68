import React,  { useEffect, useState } from 'react'
import Pagination from '../../../datatables/pagination'
import { Translate, withLocalize } from 'react-localize-redux';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFilePdf, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import Select from 'react-select';
import { Label, Button } from 'reactstrap'
import { PulseLoader } from 'react-spinners';
import { getPronunciationLessons, getLessonPDF } from '../../../../../api/teachers';
import LoadingFullWindow from '../../../../shared-module-components/loader-windows/loader-full-window';
import "./style.css";

const PronunciationLessons = ({ translate, onLessonsChange }) => {
  const [lessons, setLessons] = useState([]);
  const [loadingLessonId, setLoadingLessonId] = useState(null);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [limiter] = useState(10);

  const [levelCode, setLevelCode] = useState(null);
  const [subjectCode, setSubjectCode] = useState(null);

  useEffect(() => {
      doSearch();
  }, [currentPage, levelCode, subjectCode]);

  const handlePageClick = (e) => {
    setCurrentPage(e);
  };

  const doSearch = async () => {
    setLoading(true);
    try {
      const response = await getPronunciationLessons(currentPage, limiter, subjectCode, levelCode);
      setLessons(response.data.items);
      setTotalPages(response.data.meta.totalPages);

      if (onLessonsChange) {
        onLessonsChange(response.data.items);
      }
  
    } catch (error) {
      console.error('Error fetching pronunciation lessons', error);
    } finally {
      setLoading(false);
    }
  }

  const handleSubjectChange = (selectedOption) => {
    const selectedCode = selectedOption ? selectedOption.value : null;

    if (currentPage !== 0) {
      setCurrentPage(0);
    }

    setSubjectCode(selectedCode); 
  };

  const handleLevelChange = (selectedOption) => {
    const selectedCode = selectedOption ? selectedOption.value : null;

    if (currentPage !== 0) {
      setCurrentPage(0);
    }
    setLevelCode(selectedCode);
  };

  const handleClearFilters = () => {
      setSubjectCode(null);
      setLevelCode(null);
      
      if (currentPage !== 0) {
        setCurrentPage(0);
      }
  };

  const handleViewPdf = async (lessonId) => {
    setLoadingLessonId(lessonId);
    try {
        const response = await getLessonPDF(lessonId);
        window.open(response.data, '_blank');
    } catch (error) {
        console.error("Error al abrir el PDF:", error);
    } finally {
        setLoadingLessonId(null);
    }
  };

  const levelCodes = [
    { name: 'Kindergarten', code: 'K'},
    { name: 'Primer Grado', code: '1'},
    { name: 'Segundo Grado', code: '2'},
    { name: 'Tercer Grado', code: '3'},
    { name: 'Cuarto Grado', code: '4'},
    { name: 'Quinto Grado', code: '5'}
  ];

  const subjectCodes = [
    { name: 'Adquisición de la lengua', code: 'adl'},
    { name: 'Español', code: 'sp'},
    { name: 'Inglés', code: 'en'},
  ];

  return (
    <>
      <div className="pron-lessons-selectors">
        <div style={{minWidth: '200px', marginTop: '10px'}}>
          <Label 
            className="font-weight-bold"
            style={{marginBottom: '3px', fontSize: '15px'}}
          >
            <Translate id="dashboards.subject"/>
          </Label>
          <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            value={subjectCode ? { 
                value: subjectCode, 
                label: subjectCodes.find(subject => subject.code === subjectCode)?.name 
            } : null}
            onChange={handleSubjectChange}
            options={subjectCodes.map(subject => ({
                value: subject.code,
                label: subject.name
            }))}
            placeholder={translate('dashboards.filter-by-subject')}
          />
        </div>

        <div style={{minWidth: '200px', marginTop: '10px'}}>
          <Label
            className="font-weight-bold"
            style={{marginBottom: '3px', fontSize: '15px'}}
          >
              <Translate id="dashboards.level"/>
          </Label>
          <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            value={levelCode ? { 
                  value: levelCode, 
                  label: levelCodes.find(level => level.code === levelCode)?.name 
                } : null}      
            onChange={handleLevelChange}
            options={levelCodes.map(level => ({
                value: level.code,
                label: level.name
            }))}
            placeholder={translate('dashboards.filter-by-level')}
          />
        </div>
        <div style={{minWidth: '50px', display: 'flex', alignItems: 'flex-end', marginTop: '20px'}}>
            <Button
                color="danger"
                onClick={handleClearFilters}
                disabled={!subjectCode && !levelCode}
                style={{ height: '38px', padding: '0 10px' }}
            >
                <FontAwesomeIcon icon={faTrashAlt} size="lg" />
            </Button>
        </div>
      </div>
      <ul className="list-group">
        {lessons.map((lesson, index) => {
          const isLoading = loadingLessonId === lesson.LessonId;
          return (
            <li
              key={lesson.id}
              style={{fontSize: '15px'}}
              className={`list-group-item list-group-item-action d-flex justify-content-between align-items-center ${index % 2 === 0 ? 'bg-white' : 'bg-light'}`}
            >
              <div>
                <strong>{lesson.LessonTitle}</strong>
                <small className="d-block">
                  <span className='font-weight-bold'><Translate id="dashboards.units" />:</span> {lesson.Units}
                </small>
                <small className="d-block title-primary">
                  <Translate id="dashboards.code-standard" />: {lesson.ConstructedFullCode}
                </small>
              </div>

              <button
                size="sm"
                className="btn-default button-primary-custom"
                outline
                onClick={() => handleViewPdf(lesson.LessonId)}
                disabled={isLoading}
              >
                {isLoading ? (
                    <PulseLoader size={6} color="#fff" />
                  ) : (
                      <>
                        <FontAwesomeIcon icon={faFilePdf} className="mr-1" />
                        <Translate id="dashboards.view-pdf" />
                      </>
                  )}
              </button>
            </li>)
        })}
        <Pagination currentPage={currentPage} totalPages={totalPages} updateParent={handlePageClick} dapper={true}/>
      </ul>
      {loading && <LoadingFullWindow />}
    </>
    );
};
export default withLocalize(PronunciationLessons);