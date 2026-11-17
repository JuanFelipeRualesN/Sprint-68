import {CURRENT_SUBJECT} from '../../constants/subject';

const currentSubject = (current) => {
  localStorage.setItem("subject", JSON.stringify(current));
  return {
      type: CURRENT_SUBJECT,
      payload: current
  };
};

export default currentSubject;