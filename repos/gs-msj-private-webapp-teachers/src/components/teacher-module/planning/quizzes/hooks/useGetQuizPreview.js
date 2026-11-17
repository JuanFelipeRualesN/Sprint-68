import { useState } from "react";
import * as quizzes from "../services/quizzes.service";

const useGetQuizPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const [message, setMessage] = useState("");

  const getQuizPreview = async (quizId, lessonId) => {
    setIsLoading(true);

    try {
      const response = await quizzes.getQuizPreview(quizId, lessonId);

      setSucceeded(true);
      setData(response.HTMLCode);
    } catch (error) {
      setSucceeded(false);
      setMessage("An error occur while getting quiz preview");
    }

    setIsLoading(false);
  };

  return {
    getQuizPreview,
    isLoading,
    succeeded,
    message,
    html: data,
  };
};

export default useGetQuizPreview;
