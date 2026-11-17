//environment for configuration
import {
  getLearningGapDiagnosticScore,
  getLearningGapReadStudentLessonStatus,
  getLearningGapRequiredSubjects
} from "../api/teachers";

const environment = process.env.REACT_APP_ENV;
export const MANUALS_AI_SPEECH_MODULE = process.env.REACT_APP_MANUALS_AI_SPEECH_MODULE;
const LEARNING_GAP_MINIMUM_REQUIRED_SUBJECT_CODES = ["mat-sp", "sp", "en"];

const CHAT_ON = true;

const STRIPE_TOKEN = process.env.REACT_APP_STRIPE_TOKEN;
const GOOGLE_TAG_MANAGER = process.env.REACT_APP_GOOGLE_TAG_MANAGER;
const CHAT_SOCKET = process.env.REACT_APP_BASE_URL_CHAT_SOCKET;
const URL_SITE_ATHENAS = process.env.REACT_APP_BASE_URL_SITE_ATHENAS;
const URL_SITE_STUDENTS = process.env.REACT_APP_BASE_URL_SITE_STUDENTS;

const SUBSCRIPTION_API = process.env.REACT_APP_SUBSCRIPTION_API;
const AI_API = process.env.REACT_APP_AI_API;
const USERS_API = process.env.REACT_APP_USERS_API;
const ADDRESS_API = process.env.REACT_APP_ADDRESS_API;
const SCHOOL_API = process.env.REACT_APP_SCHOOL_API;
const GS_WEB_API = process.env.REACT_APP_GSWEB_API;
const MESSAGES_API = process.env.REACT_APP_MESSAGES_API;
const TEACHERS_API = process.env.REACT_APP_TEACHERS_API;
const ATHENAS_API = process.env.REACT_APP_ATHENAS_API;
const DATAWAREHOUSE_API = process.env.REACT_APP_DATAWAREHOUSE_API;
const VERSION = process.env.REACT_APP_VERSION_APP;
const PROJECT_ENVIRONMENT = process.env.REACT_APP_PROJECT_ENVIRONMENT;
const PROJECT_VERSION = process.env.REACT_APP_PROJECT_VERSION;
const PROJECT_NAME = process.env.REACT_APP_PROJECT_NAME;
const PROJECT_HAS_AVANZA = process.env.REACT_APP_PROJECT_HAS_AVANZA === "true";
const PHONE_NUMBER = process.env.REACT_APP_VERSION_PHONE_NUMBER;
const HTTP_USER_MANUAL_TEACHERS = process.env.REACT_APP_HTTP_USER_MANUAL_TEACHERS;
const HTTP_USER_MANUAL_ADMIN_ORE = process.env.REACT_APP_HTTP_USER_MANUAL_ADMIN_ORE;
const HTTP_USER_MANUAL_ADMIN = process.env.REACT_APP_HTTP_USER_MANUAL_ADMIN;
const HTTP_USER_MANUAL_DIRECTOR = process.env.REACT_APP_HTTP_USER_MANUAL_DIRECTOR;
const HTTP_USER_MANUAL_PARENT = process.env.REACT_APP_HTTP_USER_MANUAL_PARENT;

const PBI_WORKSPACE_ID = process.env.REACT_APP_PBI_WORKSPACE_ID;
const PBI_REPORT_USER_ID = process.env.REACT_APP_PBI_REPORT_USER_ID;
const PBI_USERS_PAGE_ID = process.env.REACT_APP_PBI_USERS_PAGE_ID;
const PBI_REPORT_ACADEMIC_ID = process.env.REACT_APP_PBI_REPORT_ACADEMIC_ID;
const PBI_ACADEMICS_PAGE_ID = process.env.REACT_APP_PBI_ACADEMICS_PAGE_ID;
const PBI_REPORT_CONNECTIONS_ID = process.env.REACT_APP_PBI_REPORT_CONNECTIONS_ID;
const PBI_USER_CONNECTIONS_PAGE_ID = process.env.REACT_APP_PBI_USER_CONNECTIONS_PAGE_ID;
const PBI_REPORT_ADMINISTRATIVE_ID = process.env.REACT_APP_PBI_REPORT_ADMINISTRATIVE_ID;
const PBI_TEACHER_ACTIVITIES_PAGE_ID = process.env.REACT_APP_PBI_TEACHER_ACTIVITIES_PAGE_ID;
const PBI_CUMULATIVE_CONNECTIONS_PAGE_ID = process.env.REACT_APP_PBI_CUMULATIVE_CONNECTIONS_PAGE_ID;

const APP_AZURE_CLIENT_ID = process.env.REACT_APP_APP_AZURE_CLIENT_ID;
const AZURE_REDIRECT_URI = process.env.REACT_APP_APP_AZURE_REDIRECT_URI;
const SUPPORT_URL = process.env.REACT_APP_SUPPORT_URL;
const IS_APP_MANAGED_BY_REQUIRED_SETTING = process.env.REACT_APP_IS_MANAGED_BY_REQUIRED;
const SUBJECTS_TO_FILTER_OUT = process.env.REACT_APP_SUBJECTS_TO_FILTER_OUT ? process.env.REACT_APP_SUBJECTS_TO_FILTER_OUT.split(",") : [];
const LEVELS_TO_FILTER_OUT = process.env.REACT_APP_LEVELS_TO_FILTER_OUT ? process.env.REACT_APP_LEVELS_TO_FILTER_OUT.split(",") : [];
const CATALOG_URL = process.env.REACT_APP_CATALOG_URL;
const COMMERCIAL_CHECKOUT_AVAILABLE = process.env.REACT_APP_CHECKOUT_AVAILABLE === "true";
export const DASHBOARD_AI_MODULE_AVAILABLE = process.env.REACT_APP_AI_DASHBOARD_AVAILABLE === "true";
export const AI_CHAT_BOT_AVAILABLE = process.env.REACT_APP_AI_BOT_AVAILABLE === "true";
/* End of Development */

export const DEFAULT_COUNTRY_ISO_ADDRESS = process.env.REACT_APP_DEFAULT_COUNTRY_ISO;
export const IMAGE_NEGATIVE_PROMPT = `
text, letters, words, typography, captions, subtitles,
watermarks, signatures, logos, random symbols, gibberish,
misspelled words, distorted text, numbers,
unnecessary objects, cluttered composition,
low quality, blurry, distorted anatomy,
extra limbs, duplicated objects, artifacts,
photorealistic textures, excessive details
`;

const TINY_MCE_LICENSE = "6z8mpir8ftb4hn9zadn5ebnb76983hbva162wzau28fop9b0"; // local: "ds10dtrtzjoz66q2l31gcwmhoq25vo32kh6q8x7y0q8pwwwf";
const TINY_MCE_URL = "https://athenas.genialskills.com/wiris.js";

const DOWNLOAD_WINDOWS_LINK = process.env.REACT_APP_DOWNLOAD_WINDOWS_LINK
const DOWNLOAD_MAC_LINK = "https://www.genialskillsservice.com:8981/assets/GenialSkillsWebMac.zip";
const DOWNLOAD_ANDROID_LINK = "https://play.google.com/store/apps/details?id=com.RequireTechnology.GenialSkillsWeb";
const DOWNLOAD_IPAD_LINK = "https://apps.apple.com/us/app/genial-skills-web-ipad-ver/id1460609057";
/**
 * Handle the basic settings as languages, endpoints, etc
 */
class Settings {

  static FORUM_TITLE_MAX_LENGTH = 100;
  static FORUM_MAX_CHARACTERS = 10000;
  static FORUM_DESCRIPTION_MAX_LENGTH = 20000;

  /* Citriom APP */
  static ENVIRONMENT = environment;
  static URL_STUDENTS_HOME = `${URL_SITE_STUDENTS}/students/home`;
  static URL_SITE_ATHENAS = URL_SITE_ATHENAS;
  static URL_SITE_STUDENTS = URL_SITE_STUDENTS;

  /* Date formats */
  static DATE_FORMAT_DATE_PICKER_DISPLAY = "MM/DD/YYYY";

  static IS_APP_MANAGED_BY_REQUIRED = IS_APP_MANAGED_BY_REQUIRED_SETTING;

  static IS_APP_MAINTENANCE = false;

  static FORUM_MODULES = {
    toolbar: [
      ['bold', 'italic', 'underline','strike', 'blockquote'],
      [{'list': 'ordered'}, {'list': 'bullet'}, {'indent': '-1'}, {'indent': '+1'}],
      ['link'],
      ['clean']
    ],
  };

  static FORUM_MAX_CHARACTERS = 10000;
  static FORUM_DESCRIPTION_MAX_LENGTH = 20000;
  static API_USERS_URL = USERS_API;
  static API_SCHOOLS_URL = SCHOOL_API;
  static API_ADDRESS_URL = ADDRESS_API;
  static API_SUBSCRIPTION_URL = SUBSCRIPTION_API;
  static API_GS_WEB_URL = GS_WEB_API;
  static API_ATHENAS_URL = ATHENAS_API;
  static API_TEACHER_URL = TEACHERS_API;
  static API_CHAT_CONTACTS = USERS_API;
  static API_MESSAGE_URL = MESSAGES_API;
  static API_DATAWAREHOUSE_URL = DATAWAREHOUSE_API;
  static API_AI_URL = AI_API;
  static SOCKET_URL = CHAT_SOCKET;
  static stripeTokenClient = STRIPE_TOKEN;
  static GOOGLE_TAG_MANAGER_ID = GOOGLE_TAG_MANAGER;
  static SUPPORT_URL = SUPPORT_URL;
  static VERSION_WEB_APP = VERSION;
  static PROJECT_ENVIRONMENT = PROJECT_ENVIRONMENT;
  static PROJECT_VERSION = PROJECT_VERSION;

  /** True when the app runs as MED Quindío, which follows the MEN lesson template. */
  static IS_MED_QUINDIO = PROJECT_VERSION?.trim() === "med-quindio";

  static PROJECT_NAME = PROJECT_NAME;
  static PROJECT_HAS_AVANZA = PROJECT_HAS_AVANZA;
  static PHONE_NUMBER = PHONE_NUMBER;
  static APP_COMMERCIAL_CHECKOUT_AVAILABLE = COMMERCIAL_CHECKOUT_AVAILABLE;

  static PBI_WORKSPACE_ID = PBI_WORKSPACE_ID;
  static PBI_REPORT_USER_ID = PBI_REPORT_USER_ID;
  static PBI_USERS_PAGE_ID = PBI_USERS_PAGE_ID;
  static PBI_REPORT_ACADEMIC_ID = PBI_REPORT_ACADEMIC_ID;
  static PBI_ACADEMICS_PAGE_ID = PBI_ACADEMICS_PAGE_ID;
  static PBI_REPORT_CONNECTIONS_ID = PBI_REPORT_CONNECTIONS_ID;
  static PBI_USER_CONNECTIONS_PAGE_ID = PBI_USER_CONNECTIONS_PAGE_ID;
  static PBI_REPORT_ADMINISTRATIVE_ID = PBI_REPORT_ADMINISTRATIVE_ID;
  static PBI_TEACHER_ACTIVITIES_PAGE_ID = PBI_TEACHER_ACTIVITIES_PAGE_ID;
  static PBI_CUMULATIVE_CONNECTIONS_PAGE_ID = PBI_CUMULATIVE_CONNECTIONS_PAGE_ID;

  static DEFAULT_PERIODS_QUANTITY = 12;
  static DEFAULT_TO_CLOSE_PERIODS_QUANTITY = 32;
  static DEFAULT_LANGUAGE = "es";
  static QUIZ_ALLOW_LEVEL_3_4 = false;
  static CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE = "4";
  static CUSTOM_QUIZ_TYPE_SELECTION = "1";
  static CUSTOM_QUIZ_TYPE_MATCH = "2";
  static CUSTOM_QUIZ_TYPE_OPEN = "3";
  static CUSTOM_QUIZ_TYPE_MIX = "5";

  static CHAT_LIVE_CHAT_LICENSE = 10895502; // Chat license
  static URL_FORM_CLICKUP_EN = "https://estudiayavanza.reqsupport.com/support/home";
  static URL_FORM_CLICKUP_SP = "https://estudiayavanza.reqsupport.com/support/home";

  static APP_AZURE_CLIENT_ID = APP_AZURE_CLIENT_ID;
  static AZURE_REDIRECT_URI = AZURE_REDIRECT_URI;

  static HTTP_GENIAL_STORE = "https://store.genialskills.com";

  static KINDER_INTERACTIVE_RESOURCE = process.env.REACT_APP_KINDER_INTERACTIVE_RESOURCE || "https://wonderful-pebble-09136350f.5.azurestaticapps.net/";

  static HTTP_USER_MANUAL_TEACHERS = HTTP_USER_MANUAL_TEACHERS;

  static HTTP_USER_MANUAL_ADMIN_ORE = HTTP_USER_MANUAL_ADMIN_ORE;

  static HTTP_USER_MANUAL_ADMIN = HTTP_USER_MANUAL_ADMIN;

  static HTTP_USER_MANUAL_DIRECTOR = HTTP_USER_MANUAL_DIRECTOR;

  static HTTP_USER_MANUAL_PARENT = HTTP_USER_MANUAL_PARENT;

  static STUDENT_TUTORIAL_ELEMENTARY_PLATFORM_FUNCTIONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-elemental-funciones-de-la-plataforma.mp4";

  static STUDENT_TUTORIAL_INTERMEDIATE_PLATFORM_FUNCTIONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-intermedia-funciones-de-la-plataforma.mp4";

  static STUDENT_TUTORIAL_SUPERIOR_PLATFORM_FUNCTIONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-superior-funciones-de-la-plataforma.mp4";

  static STUDENT_TUTORIAL_ELEMENTARY_PARTS_OF_LESSONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-elemental-partes-de-la-leccion.mp4";

  static STUDENT_TUTORIAL_INTERMEDIATE_PARTS_OF_LESSONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-intermedia-partes-de-la-leccion.mp4";

  static STUDENT_TUTORIAL_SUPERIOR_PARTS_OF_LESSONS =
    "https://d2o4f9fv1zp6rq.cloudfront.net/tutorial-estudiante-superior-partes-de-la-leccion.mp4";
  
  static ESTUDIA_AVANZA_STUDENT_ELEMENTARY =
    "https://d2o4f9fv1zp6rq.cloudfront.net/estudia-y-avanza-estudiante-elemental.mp4";

  static ESTUDIA_AVANZA_STUDENT_INTERMEDIATE =
    "https://d2o4f9fv1zp6rq.cloudfront.net/estudia-y-avanza-estudiante-intermedia.mp4";

  static ESTUDIA_AVANZA_STUDENT_SUPERIOR =
    "https://d2o4f9fv1zp6rq.cloudfront.net/estudia-y-avanza-estudiante-superior.mp4";
  
  static LESSON_CATALOG_PDF_URL = CATALOG_URL;
//    "https://d2o4f9fv1zp6rq.cloudfront.net/catalogo-estudia-y-avanza.pdf";

  static SCHOOL_MODULE_SCHOOL_TYPE_ID = 1;

  static SCHOOL_MODULE_PROGRAM_TYPE_ID = 2;

  static SCHOOL_MODULE_INSTITUTION_TYPE_ID = 3;

  static SCHOOL_MODULE_COMPANY_TYPE_ID = 4;

  static BILL_SUB_STRIPE_CODE = "STR";

  static BILL_SUB_PRIVATE_CODE = "PRI";


  static ROLE_MAIN_CLIENT_ID = "1";

  static ROLE_ADMIN_ID = "2";

  static ROLE_STUDENT_ID = "3";

  static ROLE_EMPLOYEE_ID = "4";

  static ROLE_TEACHER_ID = "5";

  static ROLE_DIRECTOR_ID = "6";

  static ROLE_PARENT_ID = "7";

  static ROLE_ADMIN_ORE_ID = "8";


  static ROLE_CLIENT_STR = "client";

  static ROLE_ADMIN_STR = "admin";

  static ROLE_STUDENT_STR = "student";

  static ROLE_EMPLOYEE_STR = "employee";

  static ROLE_TEACHER_STR = "teacher";

  static ROLE_DIRECTOR_STR = "director";

  static ROLE_PARENT_NO_CLIENT_STR = "parent";

  static ROLE_ADMIN_ORE_STR = "admin-ore";

  static FORUM_MODULES = {
    toolbar: [
      ['bold', 'italic', 'underline','strike', 'blockquote'],
      [{'list': 'ordered'}, {'list': 'bullet'}, {'indent': '-1'}, {'indent': '+1'}],
      ['link'],
      ['clean']
    ],
  };

  static LANGUAGES = [
    { name: "English", code: "en" },
    { name: "Español", code: "es" },
  ];

  static DIRECTOR_ALLOWED_ROLES = [
    Settings.ROLE_STUDENT_STR,
    Settings.ROLE_TEACHER_STR,
  ];

  /** To get the text editor configs.
   * WIRIS is not available for clients outside required.
   * @returns {string}
   */
  static getTextEditorConfigs() {
    let textEditorConfigs = 'undo redo | indent bold italic underline forecolor table numlist bullist | alignleft aligncenter alignright | code |';
    if (Settings.IS_APP_MANAGED_BY_REQUIRED) {
      textEditorConfigs += " tiny_mce_wiris_formulaEditor "
    }
    textEditorConfigs += "addColonsButton ";
    return textEditorConfigs;
  }

  static getCheckoutStatus() {
    return Settings.APP_COMMERCIAL_CHECKOUT_AVAILABLE;
  }
  static getSubjectsToFilterOut() {
    return SUBJECTS_TO_FILTER_OUT;
  }

    static getLevelsToFilterOut() {
    return LEVELS_TO_FILTER_OUT;
    }

  /**
   * Returns the app version.
   * @returns {string}
   */
  static getAppVersion(){

    return Settings.VERSION_WEB_APP;
  }

  /**
   * Returns the default configuration settings for Quill editor.
   */
  static getDefaultQuillConfigs() {
    return {
      showToolBar: true,
      size: false,
      header: true,
      undo: true,
      redo: true,
      format: {
        bold: true,
        italic: true,
        underline: true,
        strike: true,
        color: true
      },
      list: {
        ordered: true,
        bullet: true,
        indent: {
          remove: true,
          add: true
        }
      },
      align: {
        left: true,
        center: true,
        right: true,
        justify: true
      },
      media: {
        video: false,
        image: false
      },
      blockquote: true,
      code: true,
      table: true,
      formula: true
    }
  }

  static getChatStatus() {
    return CHAT_ON;
  }

  static getRegexScore() {
    return /^(?!.*[eE])(?:\d+(\.\d{0,2})?|\.\d{1,2})?$/;
  }

  /**
   * Tiny MCE License
   * @returns {string}
   */
  static getTinyMCELicense() {
    return TINY_MCE_LICENSE;
  }

  /**
   * To get the minimum learning gaps.
   * @returns {string[]}
   */
  static getLearningGapMinimumSubjectsRequired(){
    return LEARNING_GAP_MINIMUM_REQUIRED_SUBJECT_CODES;
  }

  /**
   * Tiny MCE Script URL
   * @returns {string}
   */
  static getTinyMCEScriptURL() {
    return TINY_MCE_URL;
  }

  /**
   * The url where the windows app is
   */
  static getDownloadWindowsAppURL() {
    return DOWNLOAD_WINDOWS_LINK;
  }

  /**
   * The url where the mac app is
   */
  static getDownloadMacAppURL() {
    return DOWNLOAD_MAC_LINK;
  }

  /**
   * THe url where the android app is
   */
  static getDownloadAndroidAppURL() {
    return DOWNLOAD_ANDROID_LINK;
  }

  /**
   * The url where the ipad app is
   */
  static getDownloadIpadsAppUrl() {
    return DOWNLOAD_IPAD_LINK;
  }

  /**
   * Returns the stripe token
   * @returns {string}
   */
  static getStripeToken() {
    return this.stripeTokenClient;
  }

  /**
   * Returns the rubric template in English.
   * @returns {string}
   */
  static getRubricTemplateEn() {
    return '<table class="ql-table-better"><temporary class="ql-table-temporary" data-class="ql-table-better"><br></temporary><tbody><tr><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-441d"><strong>Description</strong></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-7xau"><br></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-rw7u"><strong>Criteria</strong></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-to51"><br></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-9ftz"><br></p></td></tr><tr><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-anxf"><br></p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-pnii">Excellent (4)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-09y1">Good (3)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;" class="ql-cell-focused"><p class="ql-table-block" data-cell="cell-uzgw">Satisfactory (2)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-detq">Needs improvement (1)</p></td></tr><tr><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><ol class="table-list-container" data-row="row-4lwt" data-width="72" data-height="40" data-style="height:40px;border-color:#ccc;" data-cell="cell-6osp"><li class="table-list" data-list="ordered"><span class="ql-ui" contenteditable="false"></span>Criteria 1...</li></ol></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-6lpd"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-wwfk"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-fa20"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-nyl0"><br></p></td></tr></tbody></table>';
  }

  /**
   * Returns the rubric template in Spanish.
   * @returns {string}
   */
  static getRubricTemplateEs() {
    return '<table class="ql-table-better"><temporary class="ql-table-temporary" data-class="ql-table-better"><br></temporary><tbody><tr><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-441d"><strong>Descripción</strong></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-7xau"><br></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-rw7u"><strong>Criterios</strong></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-to51"><br></p></td><td data-row="row-5yss" width="72" style="height:40px;background-color:#c4c4c4;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-9ftz"><br></p></td></tr><tr><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-anxf"><br></p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-pnii">Excelente (4)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-09y1">Bueno (3)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;" class="ql-cell-focused"><p class="ql-table-block" data-cell="cell-uzgw">Satisfactorio (2)</p></td><td data-row="row-p95h" width="72" style="background-color:#e3e3e3;height:60px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-detq">Necesita mejorar (1)</p></td></tr><tr><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><ol class="table-list-container" data-row="row-4lwt" data-width="72" data-height="40" data-style="height:40px;border-color:#ccc;" data-cell="cell-6osp"><li class="table-list" data-list="ordered"><span class="ql-ui" contenteditable="false"></span>Criterio 1...</li></ol></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-6lpd"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-wwfk"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-fa20"><br></p></td><td data-row="row-4lwt" width="72" height="40" style="height:40px;border-color:#ccc;"><p class="ql-table-block" data-cell="cell-nyl0"><br></p></td></tr></tbody></table>';
  }

  /**
   * Returns the endpoint of the users that can be handled.
   * @returns {string}
   */
  static getHandledUsersAPI() {
    return `${this.API_USERS_URL}/usersapi/profile-connections/handled-profiles/details`;
  }

  /**
   * Returns the cancellation logs API
   * @param subscriptionId The subscription Id
   * @returns {string}
   */
  static getSubscriptionCancellationLogsAPI(subscriptionId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/admin/cancellation/${subscriptionId}`;
  }

  /**
   * Returns the client general subscription data.
   * @returns {string}
   */
  static getClientSubscriptionsGeneralDataAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/subscription/user/general`;
  }

  /**
   * Gets the subscription details by the client
   * @returns {string}
   */
  static getSubscriptionDetailsClientsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/clients/subscription-details`;
  }

  /**
   * Returns the API of reports subscriptions
   * @returns {string}
   */
  static getReportSubscriptionsDetailedAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/reports/subscriptions/details`;
  }

  /**
   * The API to get the report usage
   * @returns {string}
   */
  static getReportUsageAPI() {
    return `${this.API_USERS_URL}/usersapi/reports/usage`;
  }

  /**
   * The API to get the report subscriptions
   * @returns {string}
   */
  static getReportSubscriptionsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/reports/subscriptions`;
  }


  /**
   * Get Quiz API
   * @param quizId QuizId
   * @returns {string} Returns the API to get Quiz
   */
  static getQuizAPI(quizId) {
    return `${this.API_ATHENAS_URL}/api/quizzes/quiz/${quizId}/true`;
  }

  /**
   * To get Athenas reading questions.
   * @returns {string}
   */
  static getAthenasReadigDiagnosticQuestions(){
    return `${this.API_ATHENAS_URL}/api/diagnostic/reading-questions`;
  }

  static getQuizAPIStudent(quizId, response) {
    return `${this.API_ATHENAS_URL}/api/quizzes/quiz/${quizId}/true?response=${response}`;
  }
  
  /**
   * To get the submit a Genial Quiz API
   * @returns {string}
   */
  static getSubmitGenialQuizAPI() {
    return `${this.API_GS_WEB_URL}/gswebapi/quiz/student/submit`;
  }

  /**
   * Get authorization code quiz generator API
   * @param teacherQuizId
   * @returns {string}
   */
  static getAuthorizationCodeQuizAPI(teacherQuizId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/authorization-code/create/${teacherQuizId}`;
  }

  /**
   * Returns the assigned lessons of the student
   * @param fromPage From Page
   * @param limiter How many per page
   * @returns {string}
   */
  static getAssignedLessonStudentsAPI(fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/student/lessons/${fromPage}/${limiter}`;
  }

  /**
   * Returns the endpoint of PDF printing.
   * @param lessonId Id of the lesson
   * @returns {string}
   */
  static getLessonPDFPrintAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/pdf/lesson/${lessonId}`;
  }

    /**
   * Returns the endpoint of PDF printing.
   * @param lessonId Id of the lesson
   * @param quizId Id of the quiz
   * @param outPutFormat The output format
   * @returns {string}
   */
  static getLessonQuizPDFPrintAPI(lessonId, quizId, outPutFormat) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/pdf/lesson-quiz/${lessonId}/${quizId}/${outPutFormat}`;
  }

  static getLessonTeachersAPI(subjectCode, levelCode, isGapClosing) {

    return `${this.API_TEACHER_URL}/teachersapi/lessons?subjectCode=${subjectCode}&levelCode=${levelCode}&isGapClosing=${isGapClosing}`;
  }

  /**
   * Returns the homework of students
   * @returns {string}
   */
  static getAssignedHomeworkStudentsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/pending/student/`;
  }

  /**
   * Get the homework details API by homework Id.
   * @param homeworkId
   * @returns {string}
   */
  static getHomeworkDetailsAPI(homeworkId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/details/${homeworkId}`
  }

  /**
   * Get the quiz file rules API for homework.
   * @returns {string}
   */
  static getQuizFileRulesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/files/file-rules/`;
  }

  /**
   * Get the API to upload a file.
   * @returns {string}
   */
  static getQuizFileUploadAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/files/upload/`;
  }

  /**
   * Get the file upload API for student quizzes.
   * @param authorizationCode Authorization code to allow the student to upload
   * @param langCode The language code.
   * @returns {string}
   */
  static getQuizFileUploadApiStudent(authorizationCode, langCode) {
    return `${this.API_TEACHER_URL}/teachersapi/files/quiz/upload/${authorizationCode}/${langCode}`;
  }

  /**
   * Get the plan lesson quiz results details.
   * @param planLessonId Plan Lesson Id.
   * @param profileId The student's id.
   * @param quizId The quiz's id.
   * @returns {string} Returns the API to get the plan lesson quiz results.
   */
  static getProfilePlanLessonQuizResults(planLessonId, profileId, quizId) {
    return `${this.API_TEACHER_URL}/teachersapi/reports/plan-lesson-id/${planLessonId}/profile/${profileId}/quiz/${quizId}/`;
  }

  /**
   * Endpoint to manually correct the quiz results.
   * @param planId The id of the plan.
   * @param planLessonId The id of the plan lesson.
   * @param profileId The profile (student) id.
   * @param quizId The quiz id.
   * @returns {string}
   */
  static getPostProfilePlanLessonQuizResults(planId, planLessonId, profileId, quizId){
    return `${this.API_TEACHER_URL}/teachersapi/planning/${planId}/planning-lesson/${planLessonId}/student/${profileId}/quiz/${quizId}/manual-correction/`
  }

  /**
   * To get the endpoint to manually edit the quiz results.
   * @param planningId
   * @param planningQuizId
   * @param studentId
   * @param quizId
   * @returns {string}
   */
  static getPostProfilePlanQuizResults(planningId, planningQuizId, studentId, quizId){
    return `${this.API_TEACHER_URL}/teachersapi/homework/planning/${planningId}/planning-quiz/${planningQuizId}/student/${studentId}/quiz/${quizId}/manual-correction`;
  }

  /**
   * Get the API to delete a file.
   * @returns {string}
   */
  static getDeleteFileAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/files/delete`;
  }
  
  /**
   * Get the API to get the file details.
   * @returns {string}
   */
  static getSignatureFileApi() {
    return `${this.API_TEACHER_URL}/teachersapi/files/get/`;
  }

  /**
   * Get Quiz Types API
   * @returns {string}
   */
  static getQuizTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/quiz-types/`;
  }

  /**
   * Get Question Types API
   * @returns {string}
   */
  static getQuestionTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/question-types/`;
  }

  /**
   * Get the API to create homework.
   * @returns {string}
   */
  static getCreateHomeworkAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/create/`;
  }

  /**
   * Get the API to modify homework.
   * @returns {string}
   */
  static getUpdateHomeworkAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/modify/`;
  }

  /**
   * The API to get the profile activity
   */
  static getProfilesActivityAPI() {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/activities`;
  }

  /**
   * To get the profile overall average API.
   * @param profileId The id of the student.
   * @param levelCode Optional: The level Code.
   * @param dateFrom Optional: from date.
   * @param dateTo Optional: to date.
   * @returns {string}
   */
  static getProfileOverallAverageAPI(profileId, levelCode = null, dateFrom = null, dateTo = null) {
    const params = new URLSearchParams();
    if (levelCode) params.append('levelCode', levelCode);
    if (dateFrom) params.append('dateFrom', dateFrom);
    if (dateTo) params.append('dateTo', dateTo);

    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/${profileId}/average${params.toString() ? '?' + params.toString() : ''}`;
  }

  /**
   * To get the profile overall per lesson average API by subject.
   * @param profileId The id of the student.
   * @param subjectCode The subject code.
   * @param levelCode Optional: The level Code.
   * @param dateFrom Optional: from date.
   * @param dateTo Optional: to date.
   * @returns {string}
   */
  static getProfileOverallLessonSubjectAverageAPI(profileId, subjectCode, levelCode = null, dateFrom = null, dateTo = null) {
    const params = new URLSearchParams();
    if (subjectCode) params.append('subjectCode', subjectCode);
    if (levelCode) params.append('levelCode', levelCode);
    if (dateFrom) params.append('dateFrom', dateFrom);
    if (dateTo) params.append('dateTo', dateTo);

    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/${profileId}/average/lessons${params.toString() ? '?' + params.toString() : ''}`;
  }

  /**
   * To get the profile lesson details.
   * @param profileId The id of the student.
   * @param lessonId The id of the lesson.
   * @returns {string}
   */
  static getProfileLessonAverageDetailsAPI(profileId, lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/${profileId}/average/lessons/${lessonId}`;
  }

  /**
   * API of emails details.
   * @returns {string}
   */
  static getEmailAccountDetailsEmailAPI() {
    return `${this.API_USERS_URL}/usersapi/accounts/email/details`;
  }

  /**
   * Get the roles to read api
   * @returns {string}
   */
  static getRolesReadAPI() {
    return `${this.API_USERS_URL}/usersapi/roles`;
  }

  static getStatusAccountAPI() {
    return `${this.API_USERS_URL}/usersapi/status`;
  }

  /**
   * The url to validate coupon
   * @returns {string}
   */
  static getCouponValidateAPI(couponCode, interval) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/coupons/validate/${couponCode}/${interval}`;
  }

  /**
   * The url where the coupon is created
   * @returns {string}
   */
  static getCouponCreateAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/coupons/create`;
  }

  /**
   * The url where the coupon is read
   * @returns {string}
   */
  static getCouponsReadAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/coupons/read`;
  }

  /**
   * endpoint of version 2 registration.
   * @returns {string}
   */
  static getRegisterPaymentV2API() {
    return `${this.API_USERS_URL}/usersapi/register/v2/register/0`;
  }

  /**
   * endpoint of version 2 registration for promo.
   * @returns {string}
   */
  static getRegisterPaymentV2PromoAPI() {
    return `${this.API_USERS_URL}/usersapi/register/v2/register/1`;
  }

  /**
   * Returns the endpoint of the subscription profiles (for employee use)
   * @returns {string}
   */
  static getSubscriptionProfilesAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles/subscriptions/profiles`;
  }

  /**
   * The endpoint to add the student to the group
   * @param groupId Id of the group
   * @param studentId Student ID
   * @returns {string}
   */
  static getAddStudentToGroupAPI(groupId, studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/add-to-group/${groupId}/${studentId}`;
  }

  /**
   * Returns the students plannings endpoint
   * @param studentId
   * @param filterByTeacher
   * @returns {string}
   */
  static getStudentsPlanningsAPI(studentId, filterByTeacher) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/student/${studentId}/filter/${filterByTeacher}`;
  }

  static getStudentsPlanningsAPIv2(studentId, filterByTeacher = false) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/student/${studentId}/filter/${filterByTeacher}?callExtraDetails=true`;
  }

  /**
   * Returns the  endpoint to delete the user from a group.
   * @param groupId
   * @param studentId
   * @returns {string}
   */
  static getAddStudentDeleteFromGroupAPI(groupId, studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/delete-from-group/${groupId}/${studentId}`;
  }

  /**
   * Returns the teacher students annotations
   * @param planningId Academic plan Id
   * @param studentId Student Id
   * @param fromPage From Page
   * @param limiter Limiter (Quantity per page).
   * @returns {string}
   */
  static getTeacherStudentAnnotationsAPI(planningId, studentId, fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/${planningId}/students/${studentId}/annotation/${fromPage}/${limiter}`;
  }

  /**
   * Gets the pending lessons
   * @param fromPage
   * @param limiter
   * @returns {string}
   */
  static getStudentLessonsPendingAPI(studentId, planningId, fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/pending/exams/tutor/student/${studentId}/${fromPage}/${limiter}?planningId=${planningId}`;
  }

  /**
   * Gets the pending students homework.
   * @param fromPage from page
   * @param limiter how many per page
   * @returns {string}
   */
  static getStudentHomeworkPendingAPI(studentId, planningId, fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/pending/tutor/student/${studentId}/${fromPage}/${limiter}?planningId=${planningId}`;
  }

  /**
   * Return the student annotations creation endpoint
   * @returns {string}
   */
  static getTeacherStudentAnnotationsCreateAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/annotation/create`;
  }

  /**
   * Returns the student planning average endpoint
   * @param planningId Academic plan id
   * @param studentId Student Id
   * @returns {string}
   */
  static getTeacherStudentPlanningAverageAPI(planningId, studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/${planningId}/student/${studentId}/average`;
  }

  /**
   * Returns the student homework details endpoint
   * @param planningId Planning Id
   * @param studentId Student Id
   * @returns {string}
   */
  static getTeacherStudentPlanningHomeworkDetailsAPI(planningId, studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/planning/${planningId}/student/${studentId}`;
  }

  /**
   * Returns the endpoint profiles attached to the subscriptions
   * @returns {string}
   */
  static getMainAccountProfilesAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles/subscriptions/client/profiles`;
  }

  /**
   * Return the endpoints to get the teacher groups
   * @param groupId Id of the group
   * @returns {string}
   */
  static getTeacherGroupDetails(groupId) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/${groupId}`;
  }

  /**
   * Return the endpoint of the quizzes created by the teacher.
   * @returns {string}
   */
  static getTeacherQuizzesAPI(onlyPublished) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/user/created/${onlyPublished}`;
  }

  /**
   * Returns the endpoint of the levels.
   * @returns {string}
   */
  static getLevelsAPI() {
    return `${this.API_ATHENAS_URL}/api/grades`;
  }

  /**
   * Returns the endpoint of the subjects.
   * @returns {string}
   */
  static getSubjectsAPI() {
    return `${this.API_ATHENAS_URL}/api/subjects`;
  }

  /**
   * Returns the endpoint of the plannings created by the user.
   * @returns {string}
   */
  static getTeacherPlanningAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/user/created`;
  }

  /**
   * Returns the teacher plans of the same school.
   * @returns {string}
   */
  static getTeacherPlansSameSchoolAPI(teacherUserId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/teacher-plans/${teacherUserId}`;
  }

  /**
 * Returns the endpoint of the plannings created by the user.
 * @returns {string}
 */
  static getInstruccionalPlanningAPI(teacherId, subjectCode, level) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/user/created?teacherId=${teacherId || ''}&subjectCode=${subjectCode || ''}&level=${level || ''}`;
  }

  /**
   * Returns the endpoint of the groups created.
   * @returns {string}
   */
  static getGroupStudentsAPI() {
    return `${this.API_USERS_URL}/usersapi/schools/users/groups/`;
  }

  /**
   * Returns the endpoint of the subgroups created.
   * @returns {string}
   */
  static getSubGroupStudentsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/0/50`;
  }

  /**
   * Returns the teacher plan calendar.
   * @returns {string}
   */
  static getTeacherPlanCalendarAPI(teacherPlanId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/dates/teacher-plans/${teacherPlanId}`;
  }

  /**
   * Returns the endpoint of received messages
   * @param fromPage From Page
   * @param limit Limiter per page
   * @param langCode Language Code
   * @returns {string}
   */
  static getReceivedMessagesAPI(fromPage, limit, langCode) {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/received/${fromPage}/${limit}/${langCode}`;
  }

  /**
   * Returns the endpoint of message file rules.
   * @returns {string}
   */
  static getMessageFilesRulesAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/files/rules`;
  }

  /**
   * Returns the endpoint to upload a file in the message.
   * @returns {string}
   */
  static getMessageFilesUploadAPI(langCode) {
    return `${this.API_MESSAGE_URL}/messagesapi/files/upload/${langCode}`;
  }

  /**
   * Return the message details endpoint
   * @param messageId The message Id to check
   * @returns {string}
   */
  static getMessageDetailsAPI(messageId) {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/${messageId}`;
  }

  /**
   * Endpoint to mark a message as read
   * @param messageId
   * @param langCode
   * @returns {string}
   */
  static getMessageMarkAsReadAPI(messageId, langCode) {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/mark-read/${messageId}/${langCode}`;
  }

  /**
   * Endpoint to get the send messages
   * @param fromPage
   * @param limit
   * @param langCode
   * @returns {string}
   */
  static getSentMessagesAPI(fromPage, limit, langCode) {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/sent/${fromPage}/${limit}/${langCode}`;
  }

  /**
   * Endpoint to get the connected users (friends)
   * @returns {string}
   */
  static getMessageConnectedUsersAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/connected-users`;
  }

  /**
   * Endpoint to send a message
   * @returns {string}
   */
  static getMessageSendAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/send`;
  }

  /**
   * Endpoint to send the messages by role.
   * @returns {string}
   */
  static getMessageSendByRoleAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/send-by-role/`;
  }

  /**
   * Endpoint to get the teacher groups
   * @param limit Limit per page
   * @param page Current page
   * @returns {string}
   */
  static getTeacherGroupsAPI(limit, page) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/${page}/${limit}`;
  }

  static getDirectorGroupsAPI(limit, page) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/director/${page}/${limit}`;
  }

  static getContactsTeacher(page, pageSize) {
    return `${this.API_CHAT_CONTACTS}/usersapi/profile-connections/shared-details/${page}/${pageSize}`;
  }

  /**
   * Endpoint to send the invitation
   * @param limit
   * @param page
   * @returns {string}
   */
  static getUserSentInvitationsAPI(limit, page) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/request/by-email/${page}/${limit}`;
  }

  static getConnectionsRequestPreValidationAPI(invitationCode) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/request/prevalidation/${invitationCode}`;
  }

  /**
   * Endpoint to get the teacher planning details.
   * @param id The id of the planning.
   * @returns {string}
   */
  static getTeacherPlanningDetailsAPI(id) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/detail/${id}`;
  }

  /**
   * Endpoint to get the teacher planning details with pagination.
   * @param id The id of the planning.
   * @param limit The limit of the page.
   * @param pageNumber The page number.
   * **/
  static getTeacherPlanningDetailsPaginationAPI(id, limit, pageNumber) {
    let url = `${this.API_TEACHER_URL}/teachersapi/planning/detail/${id}`;
    const params = [];

    if (limit !== undefined && limit !== null) {
      params.push(`limit=${limit}`);
    }
    if (pageNumber !== undefined && pageNumber !== null) {
      params.push(`pageNumber=${pageNumber}`);
    }

    if (params.length > 0) {
      url += `?${params.join("&")}`;
    }

    return url;
  }

  /**
   * Endpoint to archive the planning.
   * @param planningId
   * @returns {string}
   */
  static getArchivePlanningAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/archive/${planningId}`;
  }

  /**
   * Endpoint to delete the invitation by email (does not delete de email recevied)
   * @param invitationId
   * @returns {string}
   */
  static getDeleteInvitationByEmailAPI(invitationId) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/request/by-email/delete/${invitationId}`;
  }

  /**
   * Endpoint to archive a group.
   * @param groupId
   * @returns {string}
   */
  static getArchiveGroupAPI(groupId) {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/archive/${groupId}`;
  }

  /**
   * Endpoint to send the plan to the students
   * @returns {string}
   */
  static getSendPlanningToStudentsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/groups/send-plan/`;
  }

  /**
   * Endpoint to send an invitation request
   * @returns {string}
   */
  static getSendInvitationRequestAPI(invitationCode) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/request/${invitationCode}`;
  }

  /**
   * Endpoint to send a invitation request to the email
   * @returns {string}
   */
  static getSendInvitationByEmailRequestAPI() {
    return `${this.API_USERS_URL}/usersapi/profile-connections/request/by-email/create`;
  }

  /**
   * Endpoint to create the plan
   * @returns {string}
   */
  static getCreatePlanningAPI(planningId = null) {
    if(typeof planningId !== undefined && planningId !== null && planningId !== ''){
      return `${this.API_TEACHER_URL}/teachersapi/v2/planning/clone`;
    }
    return `${this.API_TEACHER_URL}/teachersapi/planning/create`;
  }

  /**
   * Endpoint to get the quiz (for student) by authorization code
   * @param authorizationCode
   * @returns {string}
   */
  static getCustomQuizAPI(authorizationCode) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/quiz/student-quiz/${authorizationCode}`;
  }

  static getQuizEditEvaluationAPI(quizId){
    return `${this.API_TEACHER_URL}/teachersapi/homework/validate-edit-published/${quizId}`;
  }

  /**
   * Endpoint to get the quiz Id (by teacher)
   * @param quizId
   * @returns {string}
   */
  static getCustomTeacherQuizAPI(quizId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/teacher-quiz/${quizId}`;
  }

  /**
   * ENdpoint to patch group name.
   * @param groupId The id of the group.
   * @returns {string}
   */
   static getUpdateGroupNameAPI(groupId)
   {
     return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/${groupId}/name/`;
   }

  /**
   * Endpoint to create group
   * @returns {string}
   */
  static getCreateGroupAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/create-group/`;
  }

  /**
   * Endpoint to archive a quiz
   * @param quizId
   * @returns {string}
   */
  static getArchiveQuizAPI(quizId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/archive/${quizId}`;
  }

  /**
   * Endpoint to duplicate a quiz
   * @param quizId
   * @returns {string}
   */
  static getDuplicateQuizAPI(quizId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/duplicate/${quizId}`;
  }

  /**
   * Endpoint to archive the lesson of the academic plan
   * @param planningId
   * @param planningLessonId
   * @returns {string}
   */
  static getArchiveLessonOfPlanningAPI(planningId, planningLessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/delete/${planningId}/planning-lesson/${planningLessonId}`;
  }

  /**
   * Endpoint to attach the quiz to the plan
   * @returns {string}
   */
  static getAttachQuizToPlanAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-quiz/`;
  }

  /**
   * Endpoint to archive the quiz of the academic plan
   * @param planningId
   * @param planningQuizId
   * @returns {string}
   */
  static getArchiveQuizOfPlanningAPI(planningId, planningQuizId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/delete/${planningId}/planning-quiz/${planningQuizId}`;
  }

  /**
   * Endpoint to attach the lesson to the plan.
   * @returns {string}
   */
  static getAttachLessonToPlanAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-lesson/`;
  }

  /**
   * Endpoint for admins to read user details
   * @returns {string}
   */
  static getUsersAdvanceRead() {
    return `${this.API_USERS_URL}/usersapi/users`;
  }

  static getUsersAdvanceAdminRead() {
    return `${this.API_USERS_URL}/usersapi/users/for-admin`;
  }

  /**
   * Endpoint to read id+email only (no name), filtered by Role/School/User. Unlike
   * for-admin, has no per-call cap.
   * @returns {string}
   */
  static getUserEmailsAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles/emails`;
  }

  /**
   * Endpoint to upload user schools
   * @returns {string}
   */
  static getUploadUserSchoolsBulk() {
    return `${this.API_USERS_URL}/usersapi/users/bulk/upload/user-schools`;
  }

  /**
   * Endpoint to modify a school entity.
   * @returns {string}
   */
  static getSchoolEntityModifyAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/entity/modify`;
  }

  /**
   * Endpoint for school entities read api.
   * @returns {string}
   */

  static getSchoolEntitiesReadAPI(currentPage, limiter, entityId, entityType, name, city, state, telephone, privateSchool) {

    // Required
    let limiterStr = "limiter=" + limiter;
    let currentPageStr = "&pageFrom=" + currentPage;

    let entityIdStr = "";
    if (typeof entityId !== 'undefined' && entityId != null) {
      entityIdStr += "&entityId=" + entityId;
    }

    let entityTypeStr = "";
    if (typeof entityType !== 'undefined' && entityType != null) {
      entityTypeStr += "&entityTypeId=" + entityType;
    }

    let nameStr = "";
    if (typeof name !== 'undefined' && name != null) {
      nameStr += "&name=" + name;
    }

    let privateSchoolStr = "";
    if (typeof privateSchool !== 'undefined') {
      privateSchoolStr += "&privatePublicSchool=" + privateSchool;
    }

    let cityStr = "";
    if (typeof city !== 'undefined' && city != null) {
      cityStr += "&city=" + city;
    }

    let stateStr = "";
    if (typeof state !== 'undefined' && state != null) {
      stateStr += "&state=" + state;
    }

    let telephoneStr = "";
    if (typeof telephone !== 'undefined' && telephone != null) {
      telephoneStr += "&phone=" + telephone;
    }
    return `${this.API_SCHOOLS_URL}/schoolsapi/entities?` + limiterStr + currentPageStr + entityIdStr + entityTypeStr + nameStr + cityStr + stateStr + telephoneStr + privateSchoolStr;
  }

  /**
   * Endpoint for free registration.
   * @returns {string}
   */
  static getRegisterFreeAPI() {
    return `${this.API_USERS_URL}/usersapi/register/free`;
  }

  /**
   * Endpoint to read school entity.
   * @returns {string}
   */
  static getSchoolEntityModelReadAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/entities/entity`;
  }

  /**
   * endpoint to read school entity details.
   * @returns {string}
   */

  static getSchoolEntityDetailsReadAPI(entityId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/entities/entity/${entityId}/details`;
  }


  /**
   * endpoints to academic details.
   * @returns {string}
   */
  static getAcademicsDetailsByEntityAPI(entityId, page, pageSize) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/entity/${entityId}?fromPage=${page}&limit=${pageSize}`;
  }

  static getDetailByAcademicDetailsAPI(entityId, entityAcademicDetailId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/entity/${entityId}/${entityAcademicDetailId}`;
  }

  static getAcademicsDetailsCreateAPI(entityId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/${entityId}/create/`;
  }

  static getAcademicsDetailsModifyAPI(entityAcademicDetailId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/modify/${entityAcademicDetailId}`;
  }

  static getAvailabilityByRecordNumberAPI(entityId, recordNo) {
    return `${this.API_USERS_URL}/usersapi/schools/validate/user_schools/${entityId}/${recordNo}`;
  }

  /***************************************/

  /**
   * endpoint to modify the entity address (schools)
   * @returns {string}
   */
  static getSchoolModuleAddressModifyAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/addresses/modify`;
  }

  /**
   * Endpoint to delete an address
   * @returns {string}
   */
  static getSchoolModuleAddressDeleteAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/addresses/delete`;
  }

  /**
   * Endpoint to modify an entity phone
   * @returns {string}
   */
  static getSchoolModulePhoneModifyAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/phones/modify`;
  }

  /**
   * Endpoint to add a relation between school entities (programs with schools, etc)
   * @returns {string}
   */
  static getSchoolModuleRelationsModifyAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/relations/add`;
  }

  /**
   * Endpoint to delete relations between school entities (schools with programs, etc)
   * @returns {string}
   */
  static getSchoolModuleRelationsDeleteAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/relations/delete`;
  }

  /**
   * Endpoint to add subscriptions to schools.
   * @returns {string}
   */
  static getSchoolModuleSubscriptionsAddAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/schools/subscriptions/add`;
  }

  /**
   * Endpoint to get a report of schools usage.
   * @returns {string}
   */
  static getSchoolModuleReportsUsageAPI() {
    return `${this.API_USERS_URL}/usersapi/reports/schools/usage`;
  }

  /**
   * Endpoint to get the plan details
   * @returns {string}
   */
  static getPlansAdvancedReadAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/advanced/read`;
  }

  /**
   * Endpoint to cancel
   * @returns {string}
   */
  static getCancellationAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/admin/cancellation`;
  }
  /**
  * Endpoint to get cancellation reasons
  * @returns {string}
  */
  static getCancellationReasonsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/cancellations/reasons`;
  }

  /**
   * Endpoint to get the related entities.
   * @returns {string}
   */
  static getSubscriptionSchoolEntitiesRelatedAPI(subscriptionId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/subscription/admin/schools-entities-related/${subscriptionId}`;
  }

  static getAcademicsDetailsAPI(subscriptionId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/subscription/read/${subscriptionId}`;
  }

  static getAcademicsDetailsBySubscriptionAPI(subscriptionId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/subscriptions/${subscriptionId}`;
  }

  /**
   * Endpoint to create a plan.
   * @returns {string}
   */
  static getPlanCreateAPI(isCommercial = false) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/create?isCommercial=${isCommercial}`;
  }

  /**
   * Endpoint to patch the planning name.
   * @param planId The id of the plan.
   * @returns {string}
   */
  static getUpdatePlanNameAPI(planId)
  {
    return `${this.API_TEACHER_URL}/teachersapi/planning/detail/${planId}/name/`;
  }

  /**
   * Endpoint to get countries full details.
   * @param filterByLanguage
   * @param onlyAvailable
   * @returns {string}
   */
  static getCountriesFullDetailsAPI(filterByLanguage, onlyAvailable) {
    return `${this.API_ADDRESS_URL}/addressapi/addresses/countries/filtering/${filterByLanguage}/${onlyAvailable}`;
  }

  /**
   * Endpoint to get the languages.
   * @returns {string}
   */
  static getLanguagesAPI() {
    return `${this.API_GS_WEB_URL}/gswebapi/language`;
  }

  /**
   * Endpoint to read the products.
   * @param onlyAvailables To get only available items
   * @param requireItemProducts If item products are needed in the response.
   * @param isCommercial If the products are commercial or not.
   * @returns {string}
   */
  static getProductsAPI(onlyAvailables, requireItemProducts, isCommercial = false) {
    const baseUrl = `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/products/filter/${onlyAvailables}/${requireItemProducts}`;
    const query = `?isCommercial=${isCommercial}`;
    return `${baseUrl}${query}`;
  }

  /**
   * Endpoint to read a specific product.
   * @param productId The ID of the product.
   * @returns {string}
   */
  static getProductSpecificAPI(productId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/products/read/${productId}`;
  }

  /**
   * Endpoint to get the billing interval.
   * @returns {string}
   */
  static getBillingIntervalAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/billing-interval/read`;
  }

  /**
   * Endpoint to get the currencies available.
   * @returns {string}
   */
  static getCurrenciesAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/currency/read`;
  }

  /**
   * Endpoint to subscribe user to plan by employee.
   * @returns {string}
   */
  static getSubscribeUserToPlanByEmployeeAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/admin/subscribe-user`;
  }

  static getSubscritionsAssociatedTeacherAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/active/related/user/`;
  }

  static getDetailsAcademicsModifyAPI(entityAcademicDetailId) {
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/details/modify/${entityAcademicDetailId}`;
  }

  static getCountryRegionsAPI(countryIsoCode) {
    let url = `${this.API_SCHOOLS_URL}/schoolsapi/regions`
    if (countryIsoCode) {
      url += `?countryIso=${countryIsoCode}`;
    }
    return url;
  }

  static getPrePostResultsTeacherAPI(teacherPlanLessonId, studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/pre_post/results/${teacherPlanLessonId}/${studentId}`;
  }

  /**
   * Endpoint to get the taxes.
   * @returns {string}
   */
  static getTaxesAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/taxes/read`;
  }

  /**
   * Endpoint to update a plan.
   * @returns {string}
   */
  static getPlanUpdateAPI(isCommercial = false) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/update?isCommercial=${isCommercial}`;
  }

  /**
   * Endpoint to get the subscription details.
   * @returns {string}
   */
  static getSubscriptionReadAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/advanced/read`;
  }

  /**
   * Endpoint to delete subscriptions of schools.
   * @returns {string}
   */
  static getSchoolModuleSubscriptionsDeleteAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/schools/subscriptions/delete`;
  }

  /**
   * Endpoint to delete a phone.
   * @returns {string}
   */
  static getSchoolModulePhoneDeleteAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/phones/delete`;
  }

  /**
   * Endpoint to get the mini games lock.
   * @param lockGame To lock the game.
   * @returns {string}
   */
  static getMiniGamesLockCreateAPI(lockGame) {
    return `${this.API_GS_WEB_URL}/gswebapi/mini-games/lock/modify/${lockGame}`;
  }

  /**
   * Endpoint to get unread messages.
   * @returns {string}
   */
  static getTotalUnreadMessagesAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/messages/unreads`;
  }
  /**
   *
   * Endpoint to modify existent plan
   * @returns
   */
  static getModifyPlanAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/modify`;
  }
  /**
   * Endpoints to block the mini games.
   * @returns {string}
   */
  static getMiniGamesBlockReaderAPI(fromParentApp) {
    return `${this.API_GS_WEB_URL}/gswebapi/mini-games/lock/status/${fromParentApp}`;
  }

  /**
   * Endpoint to get the items main account.
   * @returns {string}
   */
  static getItemsSubscribedUsersAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/item-products/main-account`;
  }

  /**
   * endpoint to get the guests plans.
   * @returns {string}
   */
  static getPlansAvailableGuestsAPI({
                                      isSchool = false,
                                      onlyAvailables = true,
                                      isPrivate = false,
                                      currencyCode = "USD",
                                      filterByLanguage = true,
                                      planId = null,
                                      specialPromo = false,
                                    } = {}) {
    const params = new URLSearchParams();

    params.append("isSchool", isSchool);
    params.append("onlyAvailables", onlyAvailables);
    params.append("isPrivate", isPrivate);
    params.append("currencyCode", currencyCode);
    params.append("filterByLanguage", filterByLanguage);
    if (planId !== null) {
      params.append("planId", planId);
    }
    params.append("specialPromo", specialPromo);

    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/guests?${params.toString()}`;
  }

  /**
   * Endpoint to get the special promos.
   * @returns {string}
   */
  static getPlansSpecialPromosAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/special_promos`;
  }

  /**
   * Endpoint to get planning cost
   * @param planId Id of the plan
   * @param stateIso The state ISO code
   * @param countryIso Country Iso Code
   * @param subscriptionQuantity The quantity
   * @param couponPlatformId The coupon
   * @returns {string}
   */
  static getPlanCostAPI(planId, subscriptionQuantity, couponPlatformId = null,
                        stateIso = null, countryIso = null) {
    const params = new URLSearchParams({
      planId,
      subscriptionQuantity,
      stateIso,
      countryIso
    });

    if (couponPlatformId !== null) {
      params.append("couponPlatformId", couponPlatformId);
    }

    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/calculate?${params.toString()}`;
  }

  /**
   * Endpoint to get the plan cost for admin API.
   * @param planId The plan id.
   * @param subscriptionQuantity The subscription's quantity.
   * @param couponPlatformId The coupon platform id.
   * @returns {string}
   */
  static getPlanCostAdminAPI(userId, planId, subscriptionQuantity, couponPlatformId = null) {
    const params = new URLSearchParams({
      userId,
      planId,
      subscriptionQuantity
    });

    if (couponPlatformId !== null) {
      params.append("couponPlatformId", couponPlatformId);
    }

    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/plans/admin/calculate?${params.toString()}`;
  }

  /**
   * Endpoint to subscribe.
   * @returns {string}
   */
  static getSubscribeUserToPlanAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/subscribe`;
  }
  /**
    * Endpoint to get subscriptions.
    * @returns {string}
    */
  static getUserActiveSubscriptionsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/active/related/user`;
  }

  /**
   * Endpoint to attach a profile.
   * @returns {string}
   */
  static getAssignSeatAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/attach-profile`;
  }

  static getRemoveSeatAPI(){
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/remove-profiles`;
  }

  /**
   * Endpoint to update the payment method.
   * @returns {string}
   */
  static getUpdatePaymentMethodAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/customers/stripe/update_payment_method`;
  }

  /**
   * Endpoint to modify user.
   * @returns {string}
   */
  static getUserNewPasswordInsertAPI() {
    return `${this.API_USERS_URL}/usersapi/accounts/password/modify`;
  }

  /**
   * Endpoint to get user main address.
   * @returns {string}
   */
  static getUserMainAddressAPI() {
    return `${this.API_USERS_URL}/usersapi/addresses/user/main`;
  }

  /**
   * Endpoint to relate the user to the school according to the subscription id.
   * @param subscriptionId Subscription Id.
   * @returns {string}
   */
  static getUserSchoolCreateRelationBySubscriptionAPI(subscriptionId) {
    return `${this.API_USERS_URL}/usersapi/schools/users/relate-user-school-by-subscription/${subscriptionId}`;
  }

  /** To get the user profile connections by pagination **/
  static getUserProfileConnectionsByPaginationAPI(fromPage, limiter, userId) {
    if (typeof userId !== 'undefined' && userId !== null && userId !== "") {
      return `${this.API_USERS_URL}/usersapi/profile-connections/shared-details/${fromPage}/${limiter}/false/${userId}`;
    }
    else {
      return `${this.API_USERS_URL}/usersapi/profile-connections/shared-details/${fromPage}/${limiter}`;
    }
  }

  /** To get the user profile connections requests by pagination **/
  static getUserProfileConnectionsRequestsByPaginationAPI(fromPage, limiter) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/requests/shared-details/${fromPage}/${limiter}`;
  }

  /**
   * Endpoint to modify an address.
   * @returns {string}
   */
  static getUserMainAddressUpdateAPI() {
    return `${this.API_USERS_URL}/usersapi/addresses/user/modify`;
  }

  /**
   * Endpoint to register a profile.
   * @returns {string}
   */
  static getRegisterProfileAPI(isHomeschool) {
    if (isHomeschool) {
      return `${this.API_USERS_URL}/usersapi/profiles/register?isHomeSchool=true`;
    }
    // Default registration endpoint
    return `${this.API_USERS_URL}/usersapi/profiles/register`;
  }



  /**
   * Endpoint to read item product subject and level interface.
   * @param itemProductId
   * @returns {string}
   */
  static getItemProductInterfaceAPI(itemProductId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/interface/item-products/${itemProductId}`;
  }

  /**
   * Endpoint to register user by admin.
   * @returns {string}
   */
  static getUserRegisterByAdminAPI() {
    return `${this.API_USERS_URL}/usersapi/register/admin/users/register`;
  }

  /**
   * Endpoint to modify user by admin.
   * @returns {string}
   */
  static getUserInfoUpdateByAdminAPI() {
    return `${this.API_USERS_URL}/usersapi/users/modify`;
  }

  /**
   * Endpoint to get lessons details.
   * @returns {string}
   */
  static getLessonDetailsAPI() {
    return `${this.API_ATHENAS_URL}/api/lessons/lesson/`;
  }

  /**
   * Endpoint to get lesson details through the Teachers API.
   * @param {string} lessonId
   * @returns {string}
   */
  static getLessonDetailsTeachersAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/teachers/${lessonId}`;
  }

  /**
   * Endpoint to get lessons details by id.
   * @param lessonId
   * @returns {string}
   */
  static getLessonDetailsByIdAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/${lessonId}`;
  }

  /**
   * Endpoint to get lesson description details
   * @returns {string}
   */
  static getLessonDescriptionDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/description`;
  }

  /**
   * Endpoint to get lesson full codes
   * @returns {string} 
   */
  static getLessonFullCodesDetailsAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/codes/constructed-codes/lesson/${lessonId}`;
  }

  /**
   * Endpoint to get lesson objectives details
   * @returns {string}
   */
  static getLessonObjectivesDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/objective`;
  }

  /**
   * Endpoint to get lesson transversal themes details
   * @returns {string}
   */
  static getLessonTransversalThemesDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/transversal-theme`;
  }

  /**
   * Endpoint to get lesson strategies details
   * @returns {string}
   */
  static getLessonStrategiesDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/strategies`;
  }

  /**
   * Endpoint to get lesson performance task details
   * @returns {string}
   */
  static getLessonPerformanceTaskDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/performance-task`;
  }

  /**
   * Endpoint to get lesson concept details
   * @returns {string}
   */
  static getLessonConceptDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/concept`;
  }

  /**
   * Endpoint to get lesson definition details
   * @returns {string}
   */
  static getLessonDefinitionDetailsAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/lessons/definition`;
  }

  /**
   * Endpoint to get lesson example details
   * @returns {string}
   */
  static getLessonExampleDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/example`;
  }

  /**
   * Endpoint to get lesson quizzes details
   * @returns {string}
   */
  static getLessonQuizzesDetailsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/quizzes-details`;
  }

  
  /**
   * Get the lessons details for student
   * @param lessonId
   * @returns {string}
   */
  static getLessonDetailsAPIGame(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}`;
  }
  /**
   * Get the lessons definitions for student
   * @param lessonId
   * @returns {string}
   */
  static getLessonDefinitions(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}/definitions`;
  }
  /**
  * Get the lessons concept for student
  * @param lessonId
  * @returns {string}
  */
  static getLessonConcept(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}/concept`;
  }
  /**
   * Get the lessons examples for student
   * @param lessonId
   * @returns {string}
   */
  static getLessonExamples(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}/examples`;
  }
  /**
   * Get the lessons practices for student
   * @param lessonId
   * @returns {string}
   */
  static getLessonPractices(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}/practices`;
  }
  /**
   * Get the lessons exams for student
   * @param lessonId
   * @returns {string}
   */
  static getLessonExams(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${lessonId}/exams`;
  }

  static getLessonExamsTeachersAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/${lessonId}/exams`;
  }


  /**
   * Endpoint to get lesson models
   * @returns {string}
   */
  static getLessonsByLessonsModelAPI() {
    return `${this.API_ATHENAS_URL}/api/lessons/by-lesson-models/true`;
  }

  /**
   * Endpoint to get syllabus
   * @returns {string}
   */
  static getSyllabusReadAPI(lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/syllabus/${lessonId}`;
  }

  /**
   * Endpoint to get user quiz results details.
   * @returns {string}
   */
  static getUserQuizDetailsAPI(profileId, userQuizId) {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/quiz/results/${profileId}/${userQuizId}`;
  }

  /**
   * get lesson details from the teacher api
   * @param profileId
   * @param quizId
   * @param planingId
   * @returns {string}
   */
  static getUserQuizDetailsTeacherAPI(profileId, quizId, planingId) {
    return `${this.API_TEACHER_URL}/teachersapi/reports/profile/quiz/results/${profileId}/${quizId}/${planingId}`;
  }

  /**
   * Endpoint to get user custom details (homeworks)
   * @param studentId
   * @param studentQuizId
   * @returns {string}
   */
  static getUserCustomQuizDetailsAPI(studentId, studentQuizId) {
    return `${this.API_TEACHER_URL}/teachersapi/homework/planning/student/${studentId}/quiz/${studentQuizId}`;
  }

  /**
   * Endpoint to get manual correction.
   * @returns {string}
   */
  static getCorrectCustomQuizManualAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/planning/student/quiz/manual/correct`;
  }

  /**
   * Endpoint to submit the custom quiz.
   * @returns {string}
   */
  static getSubmitStudentCustomQuiz() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/student-quiz/correction`;
  }

  /**
   * Endpoint to get the exam
   * @param lessonId Lesson Id
   * @returns {string}
   */
  static getLessonExamsAPI(lessonId) {
    return `${this.API_ATHENAS_URL}/api/lessons/${lessonId}/quizzes/`;
  }

  /**
   * Endpoint to get the exam by a planning
   * @param planningLessonId The planning lesson id.
   * @returns {string}
   */
  static getPlanningLessonExamsAPI(planningLessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/plan-lesson-detail/${planningLessonId}/quizzes`;
  }

  static getLessonPreviewHtmlAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/teachers/html/${lessonId}`;
  }

  /**
   * To get the quizzes tries.
   * @param profileId The profile's id.
   * @param quizId The quiz's id.
   * @returns {string}
   */
  static getUserQuizzesTriesAPI(profileId, quizId) {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/quiz/tries/${profileId}/${quizId}`;
  }

  /**
   * Endpoint to update the password.
   * @returns {string}
   */
  static getProfileUpdatePasswordAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles/password/modify`;
  }

  /**
   * Endpoint to create an activity report.
   * @returns {string}
   */
  static getActivityReportAPI() {
    return `${this.API_USERS_URL}/usersapi/activity/create`;
  }

  /**
   * Endpoint to reset password.
   * @returns {string}
   */
  static getPasswordResetNewPasswordAPI() {
    return `${this.API_USERS_URL}/usersapi/accounts/password/reset/modify`;
  }

  /**
   * Endpoint to get password reset.
   * @returns {string}
   */
  static getPasswordResetPetitionAPI() {
    return `${this.API_USERS_URL}/usersapi/accounts/password/reset/petition`;
  }

  /**
   * Endpoint to register contact.
   * @returns {string}
   */
  static getContactDetailsRegisterV2API() {
    return `${this.API_USERS_URL}/usersapi/register/contact`;
  }

  /**
   * Endpoint to set user password on registration
   * @returns {string}
   */
  static getPasswordUpdateRegisterV2API() {
    return `${this.API_USERS_URL}/usersapi/register/password`;
  }

  /**
   * Endpoint to modify profile password
   * @returns {string}
   */
  static getProfileUpdateInformationAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles/profile/modify`;
  }

  /**
   * Endpoint to get the profile information by client.
   * @returns {string}
   */
  static getProfileInformationReadAPI(profileId) {
    return `${this.API_USERS_URL}/usersapi/profiles/${profileId}`;
  }

  /**
   * Endpoint to get profiles.
   * @returns {string}
   */
  static getProfilesAPI() {
    return `${this.API_USERS_URL}/usersapi/profiles`;
  }

  /**
   * Gets the item products API.
   * @returns {string}
   */
  static getItemProductsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/item-products`;
  }

  /**
   * Get the specific product id.
   * @param productId Id of the product
   * @returns {string}
   */
  static getSpecificProductAPI(productId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/products/read/${productId}`;
  }

  /**
   * Returns a specific product id.
   * @param id
   * @returns {string}
   */
  static getSpecificItemProductAPI(id) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/item-products/${id}`;
  }

  /**
   * Returns the item product API.
   * @returns {string}
   */
  static getUpdateItemProductAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/item-products/modify`;
  }

  /**
   * Returns the item product create API.
   * @returns {string}
   */
  static getCreateProductAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/products/create`;
  }

  /**
   * Returns the item product modify API.
   * @returns {string}
   */
  static getModifyProductAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/products/modify`;
  }
  /**
   * Returns the modify teacher lesson API.
   * @returns {string}
   */
  static getModifyTeacherLessonAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-lesson/modify`;
  }

  static getPlanLessonDetailAPI(planLessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/plan-lesson-detail/${planLessonId}`;
  }

  /**
 * Returns the modify teacher quiz API.
 * @returns {string}
 */
  static getModifyTeacherQuizAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-quiz/modify`;
  }

  /*
   * Returns the API to create the Item product.
   * @returns {string}
   */
  static getItemProductCreateAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/item-products/create`;
  }

  /**
   * Endpoints to get the profile courses
   * @returns {string}
   */
  static getProfileProductsAPI(profileId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/profiles/${profileId}/items`;
  }
  /**
   * Endpoints to get the profile courses
   * @returns {string}
   */
  static getTermsAcademicsAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/terms`;
  }
  /**
   * Endpoint to get user subscription
   * @returns {string}
   */
  static getUserSubscriptionsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/clients`;
  }

  /**
   * Endpoint to get user subscription courses and grades.
   * @returns {string}
   */
  static getUserSubscriptionProductsAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/user/available-courses-grades`;
  }

  /**
   * Endpoint to get the student average.
   * @returns {string}
   */
  static getStudentAverageAPI(profileId, subjectCode, levelCode) {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/average/overall/${profileId}/${subjectCode}/${levelCode}`;
  }

  /**
   * To get the endpoint of the reports by lesson.
   * @param profileId The profile's id.
   * @param lessonId The lesson's id.
   * @returns {string}
   */
  static getStudentLessonAverageApi(profileId, lessonId) {
    return `${this.API_GS_WEB_URL}/gswebapi/reports/profile/average/lesson/${profileId}/${lessonId}`;
  }

  static getStudentLessonAverageAPITeacher(planId, profileId, lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/reports/profile/average/${planId}/profile/${profileId}/lesson/${lessonId}`;
  }

  /**
   * Endpoint to check username availability.
   * @returns {string}
   */
  static getUsernameAvailabilityAPI(username) {
    return `${this.API_USERS_URL}/usersapi/accounts/username/availability?username=${username}`;
  }

  /**
   * Endpoint to get email availability.
   * @returns {string}
   */
  static getEmailAvailabilityAPI(email) {
    return `${this.API_USERS_URL}/usersapi/accounts/email/availability?email=${email}`;
  }

  /**
   * Endpoint of login client.
   * @returns {string}
   */
  static getLoginClientAPI() {
    return `${this.API_USERS_URL}/usersapi/login/client`;
  }

  static getLoginSsoAPI() {
    return `${this.API_USERS_URL}/usersapi/login/sso`;
  }

  /**
   * Endpoint to get countries
   * @param countryIso
   * @param langCode
   * @returns {string}
   */
  static getCountriesAPI(countryIso, langCode) {

    let url = `${this.API_GS_WEB_URL}/gswebapi/addresses/app-available`;

    if (typeof countryIso !== 'undefined')
      url += `?countryIsoAlpha3=${countryIso}`;
    return url;
  }

  /**
   * Endpoint update entity subscription API.
   * @returns {string}
   */
  static getUpdateEntitySubscriptionAPI() {
    return `${this.API_SCHOOLS_URL}/schoolsapi/subscription/relate`;
  }

  /**
   * Endpoint to get the state provinces.
   * @param countryIso Country ISO Code
   * @returns {string}
   */
  static getStateProvincesAPI(countryIso) {
    return `${this.API_ADDRESS_URL}/addressapi/addresses/states/${countryIso}`;
  }

  /**
   * Endpoint to check
   * @returns {string}
   */
  static getCheckUsersPlanningAvailableAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/planning-available-students/`;
  }

  /**
   * Returns the rendpoint to delete the users relationship.
   * @param userIdA UserId to delete relation
   * @param userIdB UserId b to delete relation. If not sent, it will delete the relationship between connected user and userIdA.
   * @returns {string}
   */
  static getProfileConnectionDeleteRequestAPI(userIdA, userIdB) {
    if (typeof userIdB !== 'undefined' && userIdB !== null && userIdB !== "") {
      return `${this.API_USERS_URL}/usersapi/profile-connections/delete-connection/${userIdA}/${userIdB}`;
    }
    else {
      return `${this.API_USERS_URL}/usersapi/profile-connections/delete-connection/${userIdA}`;
    }
  }

  /**
   * Gets the profile connection to add by admin api.
   * @param userIdA User Id A.
   * @param userIdB User Id B.
   * @param userAIsAlpha If user id a is alpha for user id b
   * @returns {string}
   */
  static getProfileConnectionAddByAdminAPI(userIdA, userIdB, userAIsAlpha) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/add-connection/${userIdA}/${userAIsAlpha}/${userIdB}`;
  }

  /**
   * To get the endpoint to delete a connection request.
   * @param userId The user to remove
   * @returns {string}
   */
  static getProfileRequestConnectionDeleteRequestAPI(userId) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/delete-request/${userId}`;
  }

  /**
   * Accepts a profile connection Request.
   * @param userId The id of the user to be accepted.
   * @returns {string}
   */
  static getProfileAcceptConnectionRequestAPI(userId) {
    return `${this.API_USERS_URL}/usersapi/profile-connections/accept-request/${userId}`;
  }

  /**
   * Endpoint to delete user school.
   * @param userId The user Id.
   * @param entityId The school Id.
   * @returns {string}
   */
  static getDeleteUserSchoolsAPI(userId, entityId) {
    return `${this.API_USERS_URL}/usersapi/schools/delete/user_schools/${userId}/${entityId}`;
  }

  /**
   * Endpoint to add user school.
   * @param userId The user Id.
   * @param entityId The school Id.
   * @param identifier The identifier (or student record).
   * @returns {string}
   */
  static getAddUserSchoolsAPI(userId, entityId, identifier) {
    return `${this.API_USERS_URL}/usersapi/schools/add/user_schools/${userId}/${entityId}/${identifier}`;
  }

  /**
   * Confirm user school.
   * @param userId
   * @param entityId
   * @param identifier
   * @returns {string}
   */
  static getConfirmUserSchoolsAPI(userId, entityId, identifier) {
    return `${this.API_USERS_URL}/usersapi/schools/confirm/user_schools/${userId}/${entityId}/${identifier}`;
  }

  /**
   * Gets the user schools.
   * @param userId The user id.
   * @returns {string}
   */
  static getSchoolUsersWithInfoAPI(userId) {
    return `${this.API_USERS_URL}/usersapi/schools/info/user_schools/${userId}`;
  }

  /**
   * Gets the forum creation api.
   * @returns {string}
   */
  static getForumCreationAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/forums/`;
  }

  /**
   * Returns the regions endpoint.
   * @param countryIsoCode The iso code of country.
   * @returns {string}
   */
  static getSchoolRegionsAPI(countryIsoCode) {
    let url = `${this.API_SCHOOLS_URL}/schoolsapi/regions`
    if (countryIsoCode) {
      url += `?countryIso=${countryIsoCode}`;
    }
    return url;
  }

  /**
   * Gets the forum modify api.
   * @param forumId Forum id.
   * @returns {string}
   */
  static getForumModifyAPI(forumId) {
    return `${this.API_TEACHER_URL}/teachersapi/forums/${forumId}`;
  }

  /**
   * Get the archive creation api.
   * @param forumId Forum id.
   * @returns {string}
   */
  static getForumArchiveAPI(forumId) {
    return `${this.API_TEACHER_URL}/teachersapi/forums/${forumId}`;
  }

  /**
   * Get forum post creation api.
   * @returns {string}
   */
  static getForumPostCreationAPI(forumId) {
    return `${this.API_TEACHER_URL}/teachersapi/forums/${forumId}/post`;
  }

  /**
   * Get forum post modify api.
   * @returns {string}
   */
  static getForumPostModifyAPI(forumId, postId) {
    return `${this.API_TEACHER_URL}/teachersapi/forums/${forumId}/post/${postId}`;
  }

  /**
   * Get forum post archive api.
   * @param forumId Id of the forum.
   * @param forumPostId Id of the post.
   * @returns {string}
   */
  static getForumPostArchiveAPI(forumId,forumPostId) {
    return `${this.API_TEACHER_URL}/teachersapi/forums/${forumId}/post/${forumPostId}`;
  }

  /**
   * Gets the forum details api.
   * @param forumId id of the forum
   * @param fromPage from page
   * @param limiter how many
   * @returns {string}
   */
  static getForumDetails = (forumId, page, pageSize, postId) => {
    if(typeof page === 'undefined' || page === null) {
        page = 1;
    }

    if(typeof pageSize === 'undefined' || pageSize === null) {
        pageSize = 10;
    }

    page = page - 1;

    let url = `${Settings.API_TEACHER_URL}/teachersapi/forums/${forumId}?fromPage=${page}&limiter=${pageSize}`;
    if (postId) {
        url += `&forumParentId=${postId}`;
    }
    return url;
};

  /**
   * Gets the forum teachers list by teacherPlanId
   * @param teacherPlanId Id of the teacher
   * @param fromPage from page
   * @param limiter how many per call.
   * @param title title of the forum.
   * @param createdAt created at date.
   * @returns {string}
   */
  static getForumsTeacher(teacherPlanId, fromPage, limiter, title, createdAt) {
    const params = new URLSearchParams();

    if (fromPage !== undefined && fromPage !== null && fromPage !== '') {
      params.append('fromPage', fromPage);
    }
    if (limiter !== undefined && limiter !== null && limiter !== '') {
      params.append('limiter', limiter);
    }
    if (title !== undefined && title !== null && title !== '') {
      params.append('title', title);
    }
    if (createdAt !== undefined && createdAt !== null && createdAt !== '') {
      params.append('createdAt', createdAt);
    }
    if (teacherPlanId !== undefined && teacherPlanId !== null && teacherPlanId !== '') {
      params.append('teacherPlanId', teacherPlanId);
    }

    const queryString = params.toString();
    return `${this.API_TEACHER_URL}/teachersapi/forums/teacher${queryString ? '?' + queryString : ''}`;
  }

  /**
   * To get the endpoint for reading permissions.
   * @returns {string}
   */
  static getPermissionsAPI() {
    return `${this.API_USERS_URL}/usersapi/permissions`;
  }

  /**
   * Returns the endpoint to read a specific permission.
   * @param id Id to read.
   * @returns {string}
   */
  static getPermissionByIdAPI(id) {
    return `${this.API_USERS_URL}/usersapi/permissions/${id}`;
  }

  /**
   * Returns the endpoint to create a new permission.
   * @returns {string}
   */
  static getPermissionCreateAPI() {
    return `${this.API_USERS_URL}/usersapi/permissions/create`;
  }

  /**
   * Returns the endpoint to modify a permission.
   * @returns {string}
   */
  static getPermissionModifyAPI() {
    return `${this.API_USERS_URL}/usersapi/permissions/modify`;
  }

  /**
   * Returns the endpoint to read a specific role.
   * @param id Id of the role.
   * @returns {string}
   */
  static getRolesByIdAPI(id) {
    return `${this.API_USERS_URL}/usersapi/roles/${id}`;
  }

  /**
   * Returns the endpoint to create a role.
   * @returns {string}
   */
  static getRolesCreateAPI() {
    return `${this.API_USERS_URL}/usersapi/roles/create`;
  }

  /**
   * Returns a endpoint to modify a role.
   * @returns {string}
   */
  static getRolesModifyAPI() {
    return `${this.API_USERS_URL}/usersapi/roles/modify`;
  }

  /**
   * Returns the endpoint of getting the students of the teacher plan.
   * @param teacherPlanId The teacher plan id.
   * @returns {string}
   */
  static getTeacherStudentsPlansAPI(teacherPlanId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/groups/teacher-plans/${teacherPlanId}`;
  }

  /**
   * Returns the endpoint to remove the student from the planning.
   * @param groupId The groupId Id.
   * @param teacherPlanId The teacher plan Id.
   * @returns {string}
   */
  static getTeacherGroupPlansRemoveAPI(groupId, teacherPlanId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/groups/remove/teacher-plans/${teacherPlanId}/${groupId}`;
  }

  /**
   * Returns the endpoint to all pendings assignments of all plans.
   * @returns {json object}
   */
  static getTeacherPendingAssignmentsForAllPlansAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/teacher/corrections/pendings`;
  }

  static getTotalPendingAssignmentsForAllPlansAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/homework/teacher/corrections/pendings/total`;
  }

  static getAllTimeZonesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/timezone`;
  }

  static getSubscriptionsActivePeriodsAPI(studentId) {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/active/periods/user?userId=${studentId}`;
  }

  static getCoursesAPI() {
    return `${this.API_SUBSCRIPTION_URL}/subscriptionsapi/subscriptions/student/courses/`;
  }

  static getClearDataCacheAPI() {
    return `${this.API_GS_WEB_URL}/gswebapi/cache/flush `;
  }

  /**
   * To get the academic years.
   * @param fromPage The page from.
   * @param limiter The page to.
   * @param dateFrom The date from.
   * @param dateTo The date to.
   * @param term The term id.
   * @param filterOutExpiredAcademicYears To filter out the academic years that are expired.
   * @returns {string} The url.
   */
  static getAcademicYearsAPI(fromPage, limiter, dateFrom, dateTo, term, filterOutExpiredAcademicYears) {
    let url = `${this.API_SCHOOLS_URL}/schoolsapi/academics/details?pageFrom=${fromPage}&limiter=${limiter}`;
    if (dateFrom) {
      url += `&dateFrom=${dateFrom}`;
    }
    if (dateTo) {
      url += `&dateTo=${dateTo}`;
    }
    if(term){
      url += `&termId=${term}`;
    }
    if(filterOutExpiredAcademicYears){
        url += `&filterOutExpiredAcademicYears=${filterOutExpiredAcademicYears}`;
    }

    return url;
  }

  static getReportsLessonsPlan({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate }) {
    let url = `${this.API_TEACHER_URL}/teachersapi/reports/group/scores/lessons/plan/${teacherPlanId}?quizLimiter=${quizLimiter}&quizFromLimiter=${quizFromLimiter}&studentLimiter=${studentLimiter}&studentFromLimiter=${studentFromLimiter}`;

    if (groupCode) {
      url += `&groupCode=${groupCode}`;
    }
    if (teacherGroupId) {
      url += `&teacherGroupId=${teacherGroupId}`;
    }
    if (fromDate) {
      url += `&fromDate=${fromDate}`;
    }
    if (toDate) {
      url += `&toDate=${toDate}`;
    }

    return url;
  }

  static getReportsAssignmentsPlan({ teacherPlanId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate }) {
    let url = `${this.API_TEACHER_URL}/teachersapi/reports/group/scores/assignments/plan/${teacherPlanId}?quizLimiter=${quizLimiter}&quizFromLimiter=${quizFromLimiter}&studentLimiter=${studentLimiter}&studentFromLimiter=${studentFromLimiter}`;

    if (groupCode) {
      url += `&groupCode=${groupCode}`;
    }
    if (teacherGroupId) {
      url += `&teacherGroupId=${teacherGroupId}`;
    }
    if (fromDate) {
      url += `&fromDate=${fromDate}`;
    }
    if (toDate) {
      url += `&toDate=${toDate}`;
    }

    return url;
  }

  static getReportsGroupAveragePlan(teacherPlanId, isAssignment) {
    let url = `${this.API_TEACHER_URL}/teachersapi/reports/group/scores/average/plan/${teacherPlanId}?isAssignment=${isAssignment}`;
    return url;
  }

  static getNotificationSettingsAPI() {
    return `${this.API_MESSAGE_URL}/messagesapi/notification-settings/`;
  }

  static createOrUpdateNotificationSettingsAPI(id) {
    return `${this.API_MESSAGE_URL}/messagesapi/notification-settings/${id}`;
  }

  static getEmailNotificationSettingsAPI(){
    return `${this.API_MESSAGE_URL}/messagesapi/email-cloud-configuration`;
  }

  static getApiKeysAPI(){
    return `${this.API_GS_WEB_URL}/gswebapi/api-key`;
  }

  static getApiKeysDetailAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/api-key/${id}`
  }

  static getApiKeysPatchSecretAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/api-key/${id}/secret`
  }

  static getApiKeysPatchDescriptionAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/api-key/${id}/description`
  }

  static getTeacherLessonsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/${planningId}/lessons`;
  }

  static getTeacherLessonsWithPaginationAPI(planningId, limit, pageNumber) {
    let url = `${this.API_TEACHER_URL}/teachersapi/planning/${planningId}/lessons`;
    const params = [];

    if (limit !== undefined && limit !== null) {
      params.push(`limit=${limit}`);
    }
    if (pageNumber !== undefined && pageNumber !== null) {
      params.push(`pageNumber=${pageNumber}`);
    }

    if (params.length > 0) {
      url += `?${params.join("&")}`;
    }

    return url;
  }


  static getTeacherLessonsDeleteAPI(planningId, quizId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/${planningId}/lesson/quizzes/${quizId}`;
  }

  static getTeacherLessonsCreationAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-quizzes`;
  }

  static getTeacherLessonsModifyAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-quizzes/modify`;
  }

  static getTeacherLessonsModifyAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/attach-quizzes/modify`;
  }

  static getTeacherPlanIntegrationsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/other-subject-integration/${planningId}`;
  }

  static getIntegrationsDeleteAPI(integrationId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/other-subject-integration/${integrationId}`;
  }

  static getIntegrationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/other-subject-integration`;
  }

  static getTeacherPlanProjectsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/initiative-project-innovation/${planningId}`;
  }
  
  static getProjectsTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/initiative-project-innovation/types`;
  }

  static getProjectsDeleteAPI(projectId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/initiative-project-innovation/${projectId}`;
  }

  static getProjectsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/initiative-project-innovation`;
  } 

  static getTeacherPlanAccommodationsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/accommodation/${planningId}`;
  }
  
  static getAccommodationsTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/accommodation/types`;
  }

  static getAccommodationsDeleteAPI(projectId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/accommodation/${projectId}`;
  }

  static getAccommodationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/accommodation`;
  }

  static getTeacherPlanMaterialsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/material/${planningId}`;
  }
  
  static getMaterialsTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/material/types`;
  }

  static getMaterialsDeleteAPI(materialId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/material/${materialId}`;
  }

  static getMaterialsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/material`;
  }

  static getTeacherPlanReflectionsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/praxis-reflection/${planningId}`;
  }
  
  static getReflectionsNotAchievingReasonsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/praxis-reflection/types`;
  }

  static getReflectionsDeleteAPI(reflectionId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/praxis-reflection/${reflectionId}`;
  }

  static getReflectionsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/praxis-reflection`;
  }

  static getTeacherPlanOpenLessonsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/custom-lesson/${planningId}`;
  }
  
  static getOpenLessonsCriteriasAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/custom-lesson/criterias`;
  }

  static getOpenLessonsDeleteAPI(lessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/custom-lesson/${lessonId}`;
  }

  static getOpenLessonsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/custom-lesson`;
  }

  static getTeacherPlanEvaluationsAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/evaluation/${planningId}`;
  }
  
  static getEvaluationsTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/evaluation/types`;
  }

  static getEvaluationsDeleteAPI(evaluationId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/evaluation/${evaluationId}`;
  }

  static getEvaluationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/evaluation`;
  }

  static getTeacherPlanStrategiesAPI(planningId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/strategy/${planningId}`;
  }
  
  static getStrategiesTypesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/strategy/types`;
  }

  static getStrategiesCategoriesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/strategy/categories`;
  }  

  static getStrategiesDeleteAPI(strategyId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/strategy/${strategyId}`;
  }

  static getStrategiesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/strategy`;
  }

  static getPreviewPdfAPI(teacherPlanId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning/${teacherPlanId}/preview/pdf/generate`;
  }  

  /// Differentiation strategies endpoints

  static getDifferentiationStrategiesTypeAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy/types`;
  }

  static getDifferentiationStrategyCategoriesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy/categories`;
  }


  static postCreateDifferentiationStrategyTypesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy/types`;
  }

  static getDifferentiationStrategiesByTeacherPlanIdAPI(teacherPlanId){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy/${teacherPlanId}`;
  }

  static postCreateTeacherPlanDifferentiationStrategiesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy`;
  }

  static putUpdateTeacherPlanDifferentiationStrategiesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy`;
  }

  static deleteTeacherPlanDifferentiationStrategiesAPI(differentiationStrategyId){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/differentiation_strategy/${differentiationStrategyId}`;
  }


//// ###########################################################################################################
// Observations endpoints

  static getTeacherPlanObservationsAPI(planningId){
      return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/observation/${planningId}`;
  }

  static deleteTeacherPlanObservationsAPI(observationId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/observation/${observationId}`;
  }

  static postCreateTeacherPlanObservationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/observation`;
  }

  static putUpdateTeacherPlanObservationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/observation`;
  }

  // ###########################################################################################################
  // Common Experiences endpoints

  static getTeacherPlanCommonExperiencesAPI(planningId){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/common-experiences/${planningId}`;
  }

  static deleteTeacherPlanCommonExperiencesAPI(commonExperienceId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/common-experiences/${commonExperienceId}`;
  }

  static postCreateTeacherPlanCommonExperiencesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/common-experiences`;
  }
  
  static putUpdateTeacherPlanCommonExperiencesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/common-experiences`;
  }


  //############################################################################################################
  // Active responses endpoints

  static getActiveResponsesTypeAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response/types`;
  }

  static getActiveResponsesByTeacherPlanIdAPI(teacherPlanId){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response/${teacherPlanId}`;
  }

  static deleteTeacherPlanActiveResponsesAPI(activeResponseId) {
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response/${activeResponseId}`;
  }

  static postCreateActiveResponseTypesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response/type`;
  }

  static postCreateTeacherPlanActiveResponsesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response`;
  }

  static putUpdateTeacherPlanActiveResponsesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response`;
  }

  static getTeacherPlanActiveResponseIncludesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/planning-free-content/active-response/includes`;
  }
  

//###########################################################################################################


  /**
   * To get the additional material of the lesson.
   * @param lessonId
   * @param page
   * @param pageSize
   * @returns {string}
   */
  static getMaterialAdditionalByLessonAPI(lessonId, page, pageSize) {
    return `${this.API_ATHENAS_URL}/api/additional-material/lesson/read/${lessonId}/${page}/${pageSize}`;
  }


  /**
   * Endpoint to get the offline material.
   * @param lessonId The lesson's id.
   * @param quizId The quiz's id.
   * @param format The format of the material.
   * @returns {string}
   */
  static getMaterialOfflineAPI(lessonId, quizId = 0, format = "pdf") {
    return `${this.API_ATHENAS_URL}/api/lessons/students/print/${lessonId}/${quizId}/${format}`;
  }

  static getMaterialAdditionalById = (materialId) => {
    return `${this.API_ATHENAS_URL}/api/additional-material/read/${materialId}`;
  };

  static getMaterialAdditionalAPI() {
    return `${this.API_ATHENAS_URL}/api/additional-material/read/`;
  };

  /**
   * endpoint to get the lesson as student.
   * @param id
   * @returns {string}
   */
  static getLessonByIdAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${id}`;
  }

  static getLessonByIdConceptAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${id}/concept`;
  }

  static getLessonByIdDefinitionsAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${id}/definitions`;
  }

  static getLessonByIdExamplesAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${id}/examples`;
  }

  static getLessonByIdPracticesAPI(id){
    return `${this.API_GS_WEB_URL}/gswebapi/lessons/${id}/practices`;
  }

  /** To handle learning gap **/
  static getLearningGapReadStudentLessonStatusAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/lessons-status`
  }

  static getLearningGapReadStudentAcademicYearScoreModelAPI(studentId) {
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/scores`
  }

  static getLearningGapReadStudentLessonQuizResultListAPI(assignedLessonId) {
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/assigned-lessons/${assignedLessonId}/results`
  }

  static getLearningGapAssignLessonToStudentAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/assign-lesson`
  }

  static getLearningGapInitialDataSettingsAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/initial-data-setting/${studentId}`;
  }

  static getLearningGapRequiredSubjectsAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/required-subjects`
  }

  static getLearningGapStudentAnnotationsAPI(studentId, fromPage, limiter){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/annotations?fromPage=${fromPage}&limiter=${limiter}`
  }

  static getLearningGapRecommendationsAPI(studentId, fromPage, limiter){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/learning-sheets?fromPage=${fromPage}&limiter=${limiter}`
  }

  static getLearningGapRecommendationPrintAPI(learningSheetId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/learning-sheet/${learningSheetId}/pdf`
  }

  static getLearningGapCreateOrUpdateGradeScoreManuallyAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/${studentId}/annual-score`
  }

  static getLearningGapStudentAnnotationPostAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student/${studentId}/annotations`
  }

  static getLearningGapPutAssignLessonToStudentAPI(assignedLessonId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/assign-lesson/${assignedLessonId}`
  }

  static getLearningGapAssignManualQuizToStudentAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/${studentId}/quiz-score`;
  }

  static getLearningGapDiagnosticPreviewAPI(subjectCode, levelCode){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/diagnostic/preview/${subjectCode}/${levelCode}`;
  }

  static getLearningGapDiagnosticScoreAPI(studentId, subjectCode = null, levelCode = null){

    let url = `${this.API_TEACHER_URL}/teachersapi/learning-gap/diagnostic/${studentId}/scores`;
    let questionMarkUsed = false;
    if(subjectCode){
        url += `?subjectCode=${subjectCode}`
        questionMarkUsed = true;
    }

    if(levelCode){
      if(!questionMarkUsed) {
        url += `?levelCode=${levelCode}`
      }
      else{
        url += `&levelCode=${levelCode}`
      }
    }

    return url;
  }

  static getPostLearningGapDiagnosticScoreAPI(studentId){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/diagnostic/${studentId}/scores/manual`
  }

  static getPostLearningGapDiagnosticTestDatesAPI(){
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/diagnostic/assign`
  }

  static getLearningGapTeacherNotificationsAPI(studentId, fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/${studentId}/notifications?fromPage=${fromPage}&limiter=${limiter}`
  }

  static getLearningGapTeacherHasNotificationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/teacher/has-notifications`
  }

  static getLearningGapStudentHasNotificationsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/learning-gap/student-has-notifications`
  }

  /// Endpoints to handle lesson catalog section
  static getAthenasSubjectsAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/subjects`;
  }

  static getAthenasGradesAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/grades`;
  }

  static getAthenasStandardsAPI(fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/codes/standards?fromPage=${fromPage}&limiter=${limiter}`;
  }

  static getAthenasExpectationsAPI(fromPage, limiter) {
    return `${this.API_TEACHER_URL}/teachersapi/codes/expectations?fromPage=${fromPage}&limiter=${limiter}`;
  }

  static getPostAthenasAdvancedLessonsAPI () {
    return `${this.API_TEACHER_URL}/teachersapi/lessons/advance`;
  }


  static getSettingsPowerDeAPI() {
    return `${this.API_GS_WEB_URL}/gswebapi/platform/settings `;
  }

  static getAccessInfoAPI() {
    return `${this.API_GS_WEB_URL}/api/tracking`;
  }

  static getPlanAccessAPI() {
    return `${this.API_GS_WEB_URL}/api/tracking-plan`;
  }

  // Data Warehouse API endpoints

  /**
   * Get the grades (levels).
   */
  static getDataWarehouseGradesAPI = () => {
    return `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/level/`;
  };

  /**
   * Get the subjects.
   */
  static getDataWarehouseSubjectsAPI = () => {
    return `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/subject/`;
  };

  /**
   * Get the roles.
   */
  static getDataWarehouseRolesAPI = () => {
    return `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/role/`;
  }

   /**
   * Get the subscription status.
   */
  static getDataWarehouseStatusAPI = () => {
    return `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/status/`;
  };

  /**
   * Get the PowerBI reports
   * @param reportId The report ID.
   * @param workspaceId The workspace ID.
   */
  static getPowerBIReportsAPI = (reportId, workspaceId) => {
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/embed-powerbi-info/`;

    let hasParameterAlready = false;

    if(reportId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `reportId=${reportId}`;
      hasParameterAlready = true;
    }

    if(workspaceId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `workspaceId=${workspaceId}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the countries.
   * @param countryIso The country ISO.
   * @param country The country name.
   */
  static getDataWarehouseCountryAPI = (countryIso, country) => {
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/country/`;

    let hasParameterAlready = false;

    if(countryIso != null && countryIso.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `countryIso=${countryIso}`;
      hasParameterAlready = true;
    }

    if(country != null && country.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `country=${country}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the OREs.
   * @param countryIso The country iso.
   */
  static getDataWarehouseOreAPI = (countryIso) => {
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/ore/`;

    let hasParameterAlready = false;

    if(countryIso != null && countryIso.trim() !== "") {
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `countryIso=${countryIso}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the states.
   * @param countryIso The country iso.
   */
  static getDataWarehouseStatesAPI = (countryIso) =>{
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/state/`;

    let hasParameterAlready = false;

    if(countryIso != null && countryIso.trim() !== "") {
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `countryIso=${countryIso}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the cities.
   * @param countryIso The country iso.
   * @param stateIso The state iso.
   * @param oreId the ore Id.
   */
  static getDataWarehouseCitiesAPI = (countryIso, stateIso, oreId) =>{
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/city/`;

    let hasParameterAlready = false;

    if(countryIso != null && countryIso.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `countryIso=${countryIso}`;
      hasParameterAlready = true;
    }

    if(stateIso != null && stateIso.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `stateIsoCode=${stateIso}`;
      hasParameterAlready = true;
    }

    if(oreId != null) {
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `oreId=${oreId}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the schools api.
   * @param schoolType School Type if PRIVATE or PUBLIC.
   * @param city City.
   * @param oreId Ore Id.
   * @param stateIsoCode State ISO Code.
   * @param countryIso Country ISO Code.
   * @param schoolIdentifier School Identifier.
   * @param limit Get limits.
   */
  static getDataWarehouseSchoolsAPI = (schoolType, city, oreId, stateIsoCode, countryIso, schoolIdentifier, limit) => {
    let url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/school/`;

    let hasParameterAlready = false;

    if(schoolType != null && schoolType.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `schoolType=${schoolType}`;
      hasParameterAlready = true;
    }

    if(city != null && city.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `city=${city}`;
      hasParameterAlready = true;
    }

    if(oreId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `oreId=${oreId}`;
      hasParameterAlready = true;
    }
    if(stateIsoCode != null && stateIsoCode.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `stateIsoCode=${stateIsoCode}`;
      hasParameterAlready = true;
    }

    if(countryIso != null && countryIso.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `countryIso=${countryIso}`;
      hasParameterAlready = true;
    }

    if(schoolIdentifier != null && schoolIdentifier.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `schoolIdentifier=${schoolIdentifier}`;
      hasParameterAlready = true;
    }

    if(limit != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `limit=${limit}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * To get the users API call.
   * @param username Username filter.
   * @param userId UserId filter
   * @param email Email filter
   * @param firstName First name filter
   * @param lastName Last name filter
   * @param roleId role Id filter
   * @param status Subscription status filter
   * @param role Role name filter
   * @param schoolId school Id filter
   * @param sie sie filter
   * @param oreId The Ore filter
   * @param callDocument If call the document to download or call the results for a data table.
   * @param limit The limit (total)
   * @param pageNumber The page number.
   */
  static getDataWarehouseUsersAPI = (username, userId, email, firstName, lastName, roleId, status, role, schoolId, sie, oreId, callDocument, limit, pageNumber) => {
    let url = '';

    if(callDocument)
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/users-document/`;
    else
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/users-list/`;
    let hasParameterAlready = false;

    if(username != null && username.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `username=${username}`;
      hasParameterAlready = true;
    }

    if(userId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `userId=${userId}`;
      hasParameterAlready = true;
    }

    if(email != null && email.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `email=${email}`;
      hasParameterAlready = true;
    }
    if(firstName != null && firstName.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `firstName=${firstName}`;
      hasParameterAlready = true;
    }

    if(lastName != null && lastName.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `lastName=${lastName}`;
      hasParameterAlready = true;
    }

    if(roleId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `roleId=${roleId}`;
      hasParameterAlready = true;
    }

    if(status != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `status=${status}`;
      hasParameterAlready = true;
    }

    if(oreId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `oreId=${oreId}`;
      hasParameterAlready = true;
    }

    if(role != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `role=${role}`;
      hasParameterAlready = true;
    }
    if(schoolId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `schoolId=${schoolId}`;
      hasParameterAlready = true;
    }

    if(sie != null && sie.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `sie=${sie}`;
      hasParameterAlready = true;
    }

    if(limit != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `limit=${limit}`;
      hasParameterAlready = true;
    }

    if(pageNumber != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `offset=${pageNumber * limit}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Returns the connected user details.
   * @param fromDate From Date. REQUIRED
   * @param toDate To Date. REQUIRED.
   * @param schoolId Id of the school if any (if principal or teacher, the school Id is going to be added on the backend).
   * @param oreId If of the Ore if any.
   * @param callDocument If call the excel document or not.
   * @param limit How many items per page.
   * @param pageNumber Number of page.
   */
  static getDataWarehouseConnectedAPI = (fromDate, toDate, schoolId, oreId, status, callDocument, limit, pageNumber) => {
    let url = '';

    if(callDocument)
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/connected-document/`;
    else
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/connected-list/`;
    let hasParameterAlready = false;

    if(fromDate != null ){
      fromDate = fromDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `fromDate=${fromDate}`;
      hasParameterAlready = true;
    }

    if(toDate != null ){
      toDate = toDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `toDate=${toDate}`;
      hasParameterAlready = true;
    }

    if(schoolId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `schoolId=${schoolId}`;
      hasParameterAlready = true;
    }

    if(oreId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `oreId=${oreId}`;
      hasParameterAlready = true;
    }

    if(status != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `status=${status}`;
      hasParameterAlready = true;
    }

    if(limit != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `limit=${limit}`;
      hasParameterAlready = true;
    }

    if(pageNumber != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `offset=${pageNumber * limit}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * To get the academics performance result.
   * @param fromDate From Date. REQUIRED.
   * @param toDate To Date. REQUIRED.
   * @param subject Subject.
   * @param level Level.
   * @param schoolId School Id.
   * @param oreId Ore Id.
   * @param callDocument If call the document or the data for datatable.
   * @param limit Quantity per page.
   * @param pageNumber Page number in pagination.
   */
  static getDataWarehouseAcademicsAPI = (fromDate, toDate, subject, level, schoolId, oreId, callDocument, limit, pageNumber) =>{
    let url = '';

    if(callDocument)
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/results-document/`;
    else
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/results-list/`;

    let hasParameterAlready = false;

    if(fromDate != null ){
      fromDate = fromDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `fromDate=${fromDate}`;
      hasParameterAlready = true;
    }

    if(toDate != null ){
      toDate = toDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `toDate=${toDate}`;
      hasParameterAlready = true;
    }

    if(subject != null && subject.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `subject=${subject}`;
      hasParameterAlready = true;
    }

    if(level != null && level.trim() !== ""){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `level=${level}`;
      hasParameterAlready = true;
    }

    if(schoolId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `schoolId=${schoolId}`;
      hasParameterAlready = true;
    }

    if(oreId != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `oreId=${oreId}`;
      hasParameterAlready = true;
    }

    if(limit != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `limit=${limit}`;
      hasParameterAlready = true;
    }

    if(pageNumber != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `offset=${pageNumber * limit}`;
      hasParameterAlready = true;
    }

    return url;

  }

  /**
   * Get the activities (connections) of the user.
   * @param fromDate From date. REQUIRED.
   * @param toDate To date. REQUIRED.
   * @param callDocument If call document or not.
   * @param limit. Quantity per page.
   * @param pageNumber Number of page in pagination.
   */
  static getDataWarehouseStudentActivitiesByTeacherAPI = (fromDate, toDate, callDocument, limit, pageNumber) => {
    let url = '';

    if(callDocument)
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/connected-document/`;
    else
      url = `${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/connected-list/`;

    let hasParameterAlready = false;

    if(fromDate != null ){
      fromDate = fromDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `fromDate=${fromDate}`;
      hasParameterAlready = true;
    }

    if(toDate != null ){
      toDate = toDate.toISOString().slice(0, 19).replace('T', ' ');
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `toDate=${toDate}`;
      hasParameterAlready = true;
    }

    if(limit != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `limit=${limit}`;
      hasParameterAlready = true;
    }

    if(pageNumber != null){
      if(url.indexOf('?') === -1)
          url += `?`;

      if(hasParameterAlready)
          url += `&`;

      url += `offset=${pageNumber * limit}`;
      hasParameterAlready = true;
    }

    return url;
  }

  /**
   * Get the lessons for dashboards module.
   * @param fromPage From page.
   * @param limiter Elements per page.
   * @param subjectCode Subject code filter.
   * @param levelCode Level code filter.
   */
  static getPronunciationLessonsAPI(fromPage = 0, limiter = 10, subjectCode, levelCode) {
    let url = `${this.API_TEACHER_URL}/readingapi/pronunciation/lessons?fromPage=${fromPage}&limiter=${limiter}`;

    if (subjectCode) {
      url += `&subjectCode=${subjectCode}`;
    }
    if (levelCode) {
      url += `&levelCode=${levelCode}`;
    }

    return url;
  }

  static postSendMessageToChatbotAPI() {
    return `${this.API_TEACHER_URL}/baseapi/chatbot/ask`;
  }

  static postSuggestionsFromChatbotAPI() {
    return `${this.API_TEACHER_URL}/baseapi/chatbot/planning/suggestions`;
  }

  /**
   * @param fromPage Current page (0-based).
   * @param limit Items per page.
   * @param title Optional title filter.
   * @param available Optional availability filter.
   * @param status Optional computed-status filter.
   * @param role Optional audience RoleId filter.
   * @param sortDescending Sort by StartDate descending instead of the default ascending.
   * @returns {string}
   */
  static getEventsAPI(fromPage = 0, limit = 10, title = null, available = null, status = null, role = null, sortDescending = false) {
    let url = `${this.API_TEACHER_URL}/teachersapi/events?fromPage=${fromPage}&limit=${limit}`;
    if (title)     url += `&title=${encodeURIComponent(title)}`;
    if (available) url += `&available=${encodeURIComponent(available)}`;
    if (status)    url += `&status=${encodeURIComponent(status)}`;
    if (role)      url += `&role=${encodeURIComponent(role)}`;
    if (sortDescending) url += `&sortDescending=true`;
    return url;
  }


  /**
   * @param eventId The event ID.
   * @returns {string}
   */
  static getEventAPI(eventId) {
    return `${this.API_TEACHER_URL}/teachersapi/events/${eventId}`;
  }


  /**
   * @returns {string}
   */
  static createEventAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/events`;
  }

  /**
   * @param eventId The event ID.
   * @returns {string}
   */
  static modifyEventAPI(eventId) {
    return `${this.API_TEACHER_URL}/teachersapi/events/${eventId}`;
  }

  /**
   * @param eventId The event ID.
   * @returns {string}
   */
  static deleteEventAPI(eventId) {
    return `${this.API_TEACHER_URL}/teachersapi/events/${eventId}`;
  }

  /**
   * Returns the endpoint of the events.
   * @param month The month.
   * @param year The year.
   * @returns {string}
   */
  static getEventsByMonthAndYearForTeachersAPI(month, year) {
    return `${this.API_TEACHER_URL}/teachersapi/events/teacher?month=${month}&year=${year}`;
  }

  /**
   * Students counting endpoint.
   * @returns {string}
   */
  static getStudentsCountingAPI() {
    return `${this.API_USERS_URL}/usersapi/profile-connections/teacher-students-count`;
  }

  /**
   * Students groups total endpoint.
   * @returns {string}
   */
  static getStudentsGroupsTotalAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/teacher-groups/total`;
  }

  /**
   * Endpoint to get the user school ids.
   * @returns {string}
   */
  static getUserSchoolIdsAPI() {
    return `${this.API_USERS_URL}/usersapi/schools/info/by_token/user_schools/school_ids`;
  }

  /**
   * Endpoint to get the current academic periods
   * @returns {string}
   */
  static getAcademicPeriodsCurrentAPI(schoolIds) {
    const idsParam = Array.isArray(schoolIds) ? schoolIds.join(",") : schoolIds;
    return `${this.API_SCHOOLS_URL}/schoolsapi/academics/periods/current?schoolIds=${idsParam}`;
  }

  /**
   * Endpoint to get the current lessons.
   * @returns {string}
   */
  static getTeachersPlansLessonsCurrentAPI() {
    return `${this.API_TEACHER_URL}/teachersapi/planning/lessons/summary`;
  }

  /**
   * To get the documents of AI module.
   */
  static getDocumentsAPI() {
    return `${this.API_AI_URL}/aiservicesapi/documents`;
  }

  /**
   * To get the document by id (for AI module).
   * @param id
   */
  static getDocumentByIdAPI(id) {
    return `${this.API_AI_URL}/aiservicesapi/documents/` + id;
  }

  /**
   * TO get the subjects from AI API.
   * @returns {string}
   */
  static getAISubjectsAPI() {
    return `${this.API_AI_URL}/aiservicesapi/AthenasCurriculum/subjects`;
  }

  /**
   * TO get the levels from AI API.
   * @returns {string}
   */
  static getAILevelsAPI() {
    return `${this.API_AI_URL}/aiservicesapi/AthenasCurriculum/levels`;
  }

  /**
   * The endpoint to call the standards, or another Athenas code type.
   * @param subject T
   * @param level
   * @param typeId Athenas code type, the backend uses type 1 (standards) when it is missing.
   * @returns {string}
   */
  static getStandardsForAI(subject, level, typeId) {
    const url = new URL(`${this.API_AI_URL}/aiservicesapi/AthenasCurriculum/codes`);

    if (subject) {
      url.searchParams.append("subjectCode", subject);
    }

    if (level) {
      url.searchParams.append("levelCode", level);
    }

    if (typeId) {
      url.searchParams.append("typeId", typeId);
    }

    return url.toString();
  }

  /**
   * The endpoint to call the lessons for AI module.
   * @param subject The subject code.
   * @param level The level code.
   * @param queryText Some terms to search.
   * @returns {string}
   */
  static getLessonsForAI(subject, level, queryText) {
    const url = new URL(`${this.API_AI_URL}/aiservicesapi/AthenasCurriculum/lessons`);

    if (subject) {
      url.searchParams.append("subjectCode", subject);
    }

    if (level) {
      url.searchParams.append("levelCode", level);
    }

    if (queryText) {
      url.searchParams.append("queryText", queryText);
    }

    return url.toString();
  }

  /**
   * The route to extract text.
   * @returns {string}
   */
  static getExtractTextAPI() {
    return `${this.API_AI_URL}/aiservicesapi/ExtractText`;
  }

  // AI Quotas endpoints
  static getQuotasSettingsAPI() {
    return `${this.API_AI_URL}/aiservicesapi/quota/settings`;
  }

  static getQuotaLimitsAPI() {
    return `${this.API_AI_URL}/aiservicesapi/quota/limits`;
  }

  // Summarizes the prompt activity (counts + flagged) of a batch of users.
  static getPromptMonitoringUsersSummaryAPI() {
    return `${this.API_AI_URL}/aiservicesapi/prompt-monitoring/users`;
  }

  // UNREAD alert total for the current viewer across all of their students
  // (token-scoped and role-aware on the backend: admin = whole platform,
  // teacher/director = their students). No body/ids; drives the global badge,
  // independent of paging.
  static getPromptMonitoringAlertsUnreadAPI(from, to) {
    const url = new URL(`${this.API_AI_URL}/aiservicesapi/prompt-monitoring/alerts/unread`);
    if (from != null && from !== "") url.searchParams.append("from", from);
    if (to != null && to !== "") url.searchParams.append("to", to);
    return url.toString();
  }

  // One page of a single user's AI tutor prompt history, newest first.
  static getPromptMonitoringUserHistoryAPI(userId, page, pageSize, from, to) {
    const url = new URL(`${this.API_AI_URL}/aiservicesapi/prompt-monitoring/user-history`);
    url.searchParams.append("userId", userId);
    if (page != null) url.searchParams.append("page", page);
    if (pageSize != null) url.searchParams.append("pageSize", pageSize);
    if (from != null && from !== "") url.searchParams.append("from", from);
    if (to != null && to !== "") url.searchParams.append("to", to);
    return url.toString();
  }
}

export default Settings;
