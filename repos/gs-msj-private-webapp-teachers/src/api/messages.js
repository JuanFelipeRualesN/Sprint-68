import axios from "./interceptors";
import Settings from "../custom-classes/settings";
import LanguageManager from "../custom-classes/language-manager";


/**
 * To get the connected users
 * @returns {Promise<any | string>}
 */
export const getConnectedUsers = () => {
    return axios.get(Settings.getMessageConnectedUsersAPI());
};

/**
 * To send a message.
 * @param roleId The role's id to send the message.
 * @param subject The message's subject.
 * @param body The message's body.
 * @param attachmentsURLArray The attachments' URL if any (array).
 * @param attachmentNameArray The attachments' names if any (array).
 * @param parentMessageId The parent message's id.
 * @returns {AxiosPromise}
 */
export const sendMessageByRole = (roleId, subject, body, attachmentsURLArray,attachmentNameArray,parentMessageId) =>{

    let messageAttachmentObjectArray = [];
    // Create email attachment
    for(let i=0; i < attachmentsURLArray.length;i++)
    {
        messageAttachmentObjectArray.push({
            'AttachmentName': attachmentNameArray[i],
            'AttachmentURL': attachmentsURLArray[i],
        });
    }

    let request = {
        'RoleId': roleId,
        'EmailMessageFullDetailModel': {
            'EmailMessageModel': {
                'Subject': subject,
                'Body': body,
                'ParentMessageId': parentMessageId,
            },
            'EmailAttachmentModelList': messageAttachmentObjectArray,
        }
    };

    return axios({
        method: "POST",
        url: Settings.getMessageSendByRoleAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * To send a message.
 * @param recipientsIdArray The recipients' ids.
 * @param subject The message's subject.
 * @param body The message's body.
 * @param attachmentsURLArray The attachments' URL if any (array).
 * @param attachmentNameArray The attachments' names if any (array).
 * @param parentMessageId The parent message's id.
 * @returns {AxiosPromise}
 */
export const sendMessage = (recipientsIdArray,subject, body, attachmentsURLArray,attachmentNameArray,parentMessageId) =>{

    let messageAttachmentObjectArray = [];
    // Create email attachment
    for(let i=0; i < attachmentsURLArray.length;i++)
    {
        messageAttachmentObjectArray.push({
            'AttachmentName': attachmentNameArray[i],
            'AttachmentURL': attachmentsURLArray[i],
        });
    }

    let recipientsObjectArray = [];
    for(let i=0; i < recipientsIdArray.length;i++)
    {
        recipientsObjectArray.push({
            'RecipientId': recipientsIdArray[i],
        });
    }

    let request = {
        'EmailMessageFullDetailModel': {
            'EmailMessageModel': {
                'Subject': subject,
                'Body': body,
                'ParentMessageId': parentMessageId,
            },
            'EmailAttachmentModelList': messageAttachmentObjectArray,
            'EmailMessageRecipientModelList': recipientsObjectArray,
        }
    };

    return axios({
        method: "POST",
        url: Settings.getMessageSendAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * To get the messages received for the user.
 * @param fromPage Current page.
 * @param limit How many items per page.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getReceivedMessages = (fromPage, limit) => {
    return axios.get(Settings.getReceivedMessagesAPI(fromPage, limit, LanguageManager.getLanguage()));
};

/**
 * Returns the message files' rules.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getMessageFileRules = () => {
    return axios.get(Settings.getMessageFilesRulesAPI());
};

/**
 * Returns the messages' details.
 * @param messageId The message's id.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getMessageDetails = (messageId) => {
    return axios.get(Settings.getMessageDetailsAPI(messageId));
};


/**
 * Mark the message as read.
 * @param messageId The message's id.
 * @returns {AxiosPromise}
 */
export const markMessageAsRead = (messageId) =>{

    return axios({
        method: "POST",
        url: Settings.getMessageMarkAsReadAPI(messageId, LanguageManager.getLanguage()),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get the messages sent by the user.
 * @param fromPage The current pagination's page.
 * @param limit How many items per page.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getSentMessages = (fromPage, limit) => {
    return axios.get(Settings.getSentMessagesAPI(fromPage, limit, LanguageManager.getLanguage()));
};

/**
 * To get the total of unread messages.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getTotalOfUnreadMessages = () => {
    return axios.get(Settings.getTotalUnreadMessagesAPI());
};