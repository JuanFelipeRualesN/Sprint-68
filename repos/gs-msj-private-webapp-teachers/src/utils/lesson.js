export const getGradesOrdinals = (grade, language) => {
  switch (grade) {
    case "k":
      if (language === "en") {
        return "kindergarten";
      }
      return "kindergarten";
    case "1":
      if (language === "en") {
        return "1st grade";
      }
      return "1er grado";
    case "2":
      if (language === "en") {
        return "2nd grade";
      }
      return "2do grado";
    case "3":
      if (language === "en") {
        return "3rd grade";
      }
      return "3.er grado";
    case "4":
      if (language === "en") {
        return "4th grade";
      }
      return "4.o grado";
    case "5":
      if (language === "en") {
        return "5th grade";
      }
      return "5to grado";
    case "6":
      if (language === "en") {
        return "6th grade";
      }
      return "6to grado";
    case "7":
      if (language === "en") {
        return "7th grade";
      }
      return "7mo grado";
    case "8":
      if (language === "en") {
        return "8th grade";
      }
      return "8vo grado";
    case "9":
      if (language === "en") {
        return "9th grade";
      }
      return "9no grado";
    case "10":
      if (language === "en") {
        return "10th grade";
      }
      return "10mo grado";
    case "11":
      if (language === "en") {
        return "11th grade";
      }
      return "11mo grado";
    case "12":
      if (language === "en") {
        return "12th grade";
      }
      return "12mo grado";
    default:
      return grade;
  }
};
