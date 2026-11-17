import { useState } from 'react';
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetAssignmentDetails = () => {
  const [assigmentDetail, setAssignmentDetail] = useState();
  const [isLoading, setIsLoading] = useState(false);

  const getAssignmentDetail = async (assignmentId) => {
    try {
      setIsLoading(true);
      const endpoint = Settings.getCustomTeacherQuizAPI(assignmentId);
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : {};

      setAssignmentDetail({
        Title: data.QuizModel.Title,
        QuizTypeId: data.QuizModel.QuizTypeId,
        Instruction: data.QuizModel.Instruction,
        QuestionRequestModelList: data.QuestionRequestModelList,
        QuizMatchInstructionsModel: data.QuizMatchInstructionsModel,
        QuizAttachmentModelList: data.QuizAttachmentModelList,
        QuizExtraInfoModel: data.QuizExtraInfoModel,
      });
      setIsLoading(false);
    } catch (error) {
      setIsLoading(false);
    }
  };

  return {
    getAssignmentDetail,
    assigmentDetail,
    isLoading,
  };
};

export default useGetAssignmentDetails;
