import axios from "./interceptors";
import Settings from "../custom-classes/settings";

export const sendMessageToChatbot = (request) => {
  return axios({
    method: "POST",
    url: Settings.postSendMessageToChatbotAPI(),
    headers: {
      "Content-Type": "application/json" 
    },
    data: {
      History: request.history,
      ConversationId: request.conversationId,
      Query: request.query,
      Module: request.module,
      Profile: request.profile,
      DetailedPlanning: request.detailedPlanning
    }
  });
};

export const getSuggestionFromChatbot = (fieldName, userInput, planningContext) => {
  return axios({
    method: "POST",
    url: Settings.postSuggestionsFromChatbotAPI(),
    headers: {
      "Content-Type": "application/json",
    },
    data: {
      FieldName: fieldName,
      UserInput: userInput,
      PlanningContext: planningContext
    }
  });
};