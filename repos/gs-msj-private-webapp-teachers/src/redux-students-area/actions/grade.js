export const currentGrade = (grade) => {
  return {
    type: "GRADE_SELECTED",
    payload: grade,
  };
};
