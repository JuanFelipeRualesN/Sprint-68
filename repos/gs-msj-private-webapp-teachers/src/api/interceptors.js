import axios from "axios";
import SessionManager from "../custom-classes/session-manager";
import Swal from "sweetalert2";
import ErrorImage2 from "../assets/images/common-icons/Error-icon-Alert.svg"
import LanguageManager from "../custom-classes/language-manager";

export const getAuthHeaders = () => ({
  Token: SessionManager.getSession() || "",
  "Accept-Language": LanguageManager.getLanguage(),
});

const instance = axios.create();

instance.interceptors.request.use((req) => {
  req.headers["Accept-Language"] = LanguageManager.getLanguage();
  req.headers["Token"] = SessionManager.getSession()
    ? SessionManager.getSession()
    : "";
  return req;
});

instance.interceptors.response.use(null, (error) => {
  if (axios.isCancel(error)) {
    return Promise.reject(error);
  }

  handleErrorResponse(error);

  return Promise.reject(error);
});

export default instance;

export const handleErrorResponse = (error) => {
  let message;

  const ErrorMessages = {
    400: LanguageManager.getLanguage() === "es" ? "Hubo un problema al procesar su solicitud" : "There was some problem, while processing your request",
    401: LanguageManager.getLanguage() === "es" ? "No autorizado, no tiene permisos" : "Unauthorized, you are not allowed",
    403: LanguageManager.getLanguage() === "es" ? "Lo sentimos, no tiene permiso para esta acción" : "Sorry, You are not allowed for this action",
    404: LanguageManager.getLanguage() === "es" ? "Ruta de API faltante o indefinida" : "API route is missing or undefined",
    405: LanguageManager.getLanguage() === "es" ? "Método de ruta de API no permitido" : "API route method not allowed",
    500: LanguageManager.getLanguage() === "es" ? "Error del servidor, por favor intente nuevamente más tarde" : "Server error, please try again later",
    request: LanguageManager.getLanguage() === "es" ? "Hubo un problema con nuestros servidores, por favor intente nuevamente más tarde" : "There is some problem with our servers, please try again later",
    other: LanguageManager.getLanguage() === "es" ? "Hubo un problema con su solicitud, por favor intente nuevamente más tarde" : "There is some problem with your request, please try again later",
  };

  if (error && error.response) {
    if (error.response.status === 400) {
      let msg = "";
      if (error.response.data.Messages && error.response.data.Messages.length) {
        error.response.data.Messages.forEach((message) => {
          msg += message + "<br/>";
        });
      } else {
        msg = ErrorMessages["400"];
      }
      message = msg;
    } else if (error.response.status === 401) {
      let msg = "";
      if (error.response.data.Messages && error.response.data.Messages.length) {
        error.response.data.Messages.forEach((message) => {
          msg += message + "<br/>";
        });
      } else {
        msg = ErrorMessages["401"];
      }
      message = msg;
      SessionManager.deleteSession();
    } else if (error.response.status === 403) {
      message = ErrorMessages["403"];
      SessionManager.deleteSession();
    } else if (error.response.status === 404) {
      message = ErrorMessages["404"];
    } else if (error.response.status === 405) {
      message = ErrorMessages["405"];
    } else if (error.response.status >= 500) {
      message = ErrorMessages["500"];
    }
  } else if (error && error.request) {
    message = ErrorMessages.request;
  } else if (error instanceof Error) {
    message = error.message;
  } else {
    message = ErrorMessages.other;
  }
  Swal.fire({
    title: LanguageManager.getLanguage() === "es" ? "¡ADVERTENCIA!" : "WARNING!",
    html: message,
    imageUrl: ErrorImage2,
    imageWidth: 400,
    imageHeight: 200,
    imageAlt: "Custom image",
  });
};
