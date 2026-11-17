import React, { useState, useEffect} from 'react'
import Select from 'react-select';
import { getProfiles } from '../../../api/users';
import { Translate } from 'react-localize-redux';
import { BeatLoader } from 'react-spinners';

// Component that display the student selector to assign an Avanza lesson
const SelectorStudentAvanza = ({students, onStudentIdSelect}) => {
  const [options, setOptions] = useState([]);
  const [value, setValue] = useState(null);
  const [loading, setLoading] = useState(null);
  const [studentsPage, setStudentsPage] = useState(0);
  const [totalPages, setTotalPages] = useState(null);
  const [studentsLimiter] = useState(10);

  useEffect(() => {
    if (students && students.length > 0) {
      const formattedOptions = students.map(student => ({
        value: student.UserId,
        label: `${student.GivenName} ${student.Surname}`
      }));
      setOptions(formattedOptions);
    }
  }, [students]);

  const handleChange = (selectedOption) => {
    setValue(selectedOption);
    if (onStudentIdSelect && selectedOption) {
      onStudentIdSelect(selectedOption.value);
    }
  }

  // Fetch more students and append to the existing options
  const fetchMoreStudents = async () => {
      try {
        // Prevent fetching if already fetched all pages
        if (totalPages !== null && studentsPage >= totalPages - 1) {
          return;
        }
        setLoading(true);
        const nextPage = studentsPage + 1;
        const response = await getProfiles(undefined, undefined, undefined, nextPage, studentsLimiter);
        
        if (response.data?.UserInfo) {
          setTotalPages(response.data.PageTotals);
          const newFormattedStudents = response.data.UserInfo.map(student => ({
            value: student.UserId,
            label: `${student.GivenName} ${student.Surname}`
          }));
          
          setOptions(prevOptions => [...prevOptions, ...newFormattedStudents]);
          setStudentsPage(nextPage);
        }
        
      } catch (error) {
        console.error("Error fetching more students:", error);
      } finally {
        setLoading(false)
      }
    }

  return (
    <>
      {!loading ? <div style={{width: '230px', marginTop: '10px'}}>
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
          value={value}
          onChange={handleChange}
          options={options}
          placeholder=""
        />
      </div>
      :
      <BeatLoader size={10} color={"#6bc4ff"}/>
      }
      <span onClick={fetchMoreStudents} style={{fontSize: '13px', fontWeight: 600, color: '#456083', paddingBottom: '10px', cursor: 'pointer'}}>
        {totalPages !== null && studentsPage >= totalPages - 1 ? 
          <Translate id="lesson-catalog.no-more-students" options={{ renderInnerHtml: false }} />
          :         
          <Translate id="lesson-catalog.show-more-students" options={{ renderInnerHtml: false }} />
        }
      </span>
    </>
  )
}

export default SelectorStudentAvanza