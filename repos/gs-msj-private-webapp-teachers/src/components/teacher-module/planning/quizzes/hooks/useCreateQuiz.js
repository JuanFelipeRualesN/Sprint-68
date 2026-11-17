import { useState } from "react";
import * as quizzes from "../services/quizzes.service";
import useGetTimeZones from "../../shared/hooks/useGetTimeZones";

function generateTimeIntervals() {
  const intervals = [];

  for (let hour = 0; hour < 24; hour++) {
    for (let minute = 0; minute < 60; minute += 15) {
      const formattedHour = (hour % 12 === 0 ? 12 : hour % 12)
        .toString()
        .padStart(2, "0");
      const formattedMinute = minute.toString().padStart(2, "0");
      const meridian = hour < 12 ? "AM" : "PM";
      intervals.push(`${formattedHour}:${formattedMinute} ${meridian}`);
    }
  }

  return intervals;
}

const useCreateQuiz = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  const { timezones, getTimeZones } = useGetTimeZones();
  const [times, setTimes] = useState([]);

  const createQuiz = async (request) => {
    setIsLoading(true);

    try {
      const response = await quizzes.createQuiz(request);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  const getLists = () => {
    getTimes();
    getTimeZones();
  };

  const getTimes = () => {
    setTimes(generateTimeIntervals());
  };

  return {
    createQuiz,
    isLoading,
    succeeded,
    getLists,
    times,
    timezones,
  };
};

export default useCreateQuiz;
