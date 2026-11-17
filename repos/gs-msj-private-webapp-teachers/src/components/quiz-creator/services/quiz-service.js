import {
  getHomeworkDetails,
  setHomework,
  updateHomework,
} from "../../../api/teachers";

import SessionManager from "../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../utils/access_logger";


const fromApiToDraft = apiData => {
  const {
    QuizModel = {},
    QuizRubricModel = {},
    QuizAttachmentModelList = [],
    QuestionRequestModelList = [],
    QuizMatchInstructionsModel = [],
  } = apiData.QuizRequestModel;

  return {
    Id          : QuizModel.Id || null,
    Title       : QuizModel.Title || "",
    Instruction : QuizModel.Instruction || "",
    Rubric      : QuizRubricModel?.Desc || "",
    CustomValue : QuizModel.CustomValue || "",
    QuizTypeId  : QuizModel.QuizTypeId || "",
    FilesCount  : QuizAttachmentModelList?.length || 0,
    QuestionLength: QuestionRequestModelList?.length || 0,
    QuestionRequestModelList: QuestionRequestModelList || [],
    QuizAttachmentModelList: QuizAttachmentModelList || [],
    QuizMatchInstructionsModel: QuizMatchInstructionsModel || [],
    Messages: apiData.Messages || []
  };
};

/**
 * Match questions are grouped by an id the client makes up. The server only
 * turns that id into a real one for the groups listed here, so leaving this out
 * makes it reach the question insert as-is and be rejected.
 *
 * Server-assigned timestamps are stripped before posting to avoid field
 * conflicts, and ServerId wins over Id so an existing row is updated instead of
 * duplicated.
 */
const toApiMatchInstructions = draft =>
  (draft.QuizMatchInstructionsModel || []).map(
    ({ CreatedAt, UpdatedAt, ServerId, Id, ...rest }) => ({
      ...rest,
      Id: ServerId ?? Id,
    })
  );

const fromDraftToApi = draft => ({
  QuizModel: {
    Id            : draft.Id ?? undefined,
    Title         : draft.Title,
    Instruction   : draft.Instruction,
    CustomValue   : draft.CustomValue,
    QuizTypeId    : draft.QuizTypeId,
  },
  QuizRubricModel: { Desc: draft.Rubric },
  QuizAttachmentModelList: draft.QuizAttachmentModelList || [],
  QuizMatchInstructionsModel: toApiMatchInstructions(draft),
  QuestionRequestModelList: draft.QuestionRequestModelList.map(q => ({
    pid: q.pid,
    QuizQuestionModel: q.QuizQuestionModel,
    QuizQuestionResponseModelList: q.QuizQuestionResponseModelList
  }))
});

const fromDraftToApiPublish = draft => ({
  QuizModel: {
    Id            : draft.Id ?? undefined,
    Title         : draft.Title,
    Instruction   : draft.Instruction,
    CustomValue   : draft.CustomValue,
    QuizTypeId    : draft.QuizTypeId,
    ReadyToPublish : "1"
  },
  QuizRubricModel: { Desc: draft.Rubric },
  QuizAttachmentModelList: draft.QuizAttachmentModelList || [],
  QuizMatchInstructionsModel: toApiMatchInstructions(draft),
  QuestionRequestModelList: draft.QuestionRequestModelList.map(q => ({
    pid: q.pid,
    QuizQuestionModel: q.QuizQuestionModel,
    QuizQuestionResponseModelList: q.QuizQuestionResponseModelList
  })),
  Published: true,
  Messages: draft.Messages || []
});


export async function fetchQuiz(id) {
  try {
    const { data } = await getHomeworkDetails(id);
    try {
      let userDetails = SessionManager.getSessionDetails();
      SendAccessLog(
      {
        UserName: userDetails.Username,
        InvitationCode: userDetails.InvitationCode,
        Role: userDetails.Roles[0],
        AreaId: "4",
        AssignmentId: quizInfo.QuizRequestModel.QuizModel.Id,
        AssignmentTitle: quizInfo.QuizRequestModel.QuizModel.Title,
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
    return fromApiToDraft(data);
  } catch (err) {
    console.error("Error to fetch quiz", err);
    throw err;
  }
}

export async function saveQuiz(draft) {
  const payload = fromDraftToApi(draft);

  try {
    const { data } = draft.Id
      ? await updateHomework(payload)
      : await setHomework(payload);

    return fromApiToDraft(data);
  } catch (err) {
    console.error("Error to save quiz", err);
    throw err;
  }
}

export async function publishQuiz(draft) {
  const payload = fromDraftToApiPublish(draft);
  try {
    const { data } = await updateHomework(payload);
    return fromApiToDraft(data);
  } catch (err) {
    console.error("Error to publish quiz", err);
    throw err;
  }
}
