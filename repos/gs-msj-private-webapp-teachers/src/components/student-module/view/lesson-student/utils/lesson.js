export const getGradesOrdinals = (grade, language) => {

  switch (grade) {
    case "k":
      if (language === "en") {
        return "kindergarten";
      }
      return "kindergarten";
    case "es":
      if (language === "en") {
        return "High school";
      }
      return "Escuela Superior";
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
    case "s":
      if (language === "en") {
        return "Art: High school";
      }
      return "Arte: Nivel superior";
    case "p1":
      if (language === "en") {
        return "Art: Elementary school";
      }
      return "Arte: Nivel elemental";
    case "p2":
      if (language === "en") {
        return "Art: Middle school";
      }
      return "Arte: Nivel intermedio";
    case "ns":
      if (language === "en") {
        return "Physical education: High school";
      }
      return "Educación física: Nivel superior";
    default:
      if (language === "en") {
        return "Other courses...";
      }
      return "Otros cursos...";
  }
};

export const assignColor = (subject) => {
  switch (subject) {
    case "en":
      return "#A34787";
    case "sp":
      return "#4762E5";
    case "mat-en":
      return "#4FC494";
    case "sci-en":
      return "#DDAD75";
    case "mat-sp":
      return "#4FC494";
    case "sci-sp":
      return "#DDAD75";
    case "sci-so":
      return "#E38080";
    case "edf-sp":
      return "#9BEB05";
    case "art-sp":
      return "#27B5D6";
    case "sld-sp":
      return "#E31224";
    default:
      return "#A8A8A8";
  }
};