import LanguageManager from '../custom-classes/language-manager';
import { formatUtcToLocal } from "../utils/utils";

class ResultsToHTMLConverter {
    static getHtmlResults(dataReturned, studentName, languages, autoCorrect, showTries) {
        const transTotalPercent = LanguageManager.getTranslation("report-student-average.total", languages);
        const transTry = LanguageManager.getTranslation("report-student-average.try", languages);
        const transLevelOfKnowledge = LanguageManager.getTranslation("report-student-average.dok", languages);
        const transStudentResponse = LanguageManager.getTranslation("report-student-average.student-answer", languages);
        const transCorrect = LanguageManager.getTranslation("report-student-average.correct", languages);
        const transIncorrect = LanguageManager.getTranslation("report-student-average.incorrect", languages);
        const transQuestion = LanguageManager.getTranslation("report-student-average.question", languages);

        const takenDate = formatUtcToLocal(dataReturned.CreatedAt, LanguageManager.getLanguage());

        const tryValue = dataReturned.TryNo ?? '';
        const dokValue = dataReturned.DoK ?? '';

        let htmlToAdd = `
      <div style="font-family: Arial, sans-serif; background-image: {BackgroundImg}; padding: 20px;">
        <table style="width: 100%; background-color: #f2f2f2; border-collapse: collapse; margin-bottom: 20px;">
          <tr style="background-color: #e0e0e0;">
            <th colspan="2" style="padding: 10px; text-align: left; font-size: 18px;">
              ${dataReturned.QuizTitle}
              <br/>
              <small>${takenDate}</small>
            </th>
          </tr>

          <tr>
            <td style="padding: 10px;"><b>👤 ${studentName}</b></td>
            <td style="padding: 10px;"><b>${transTotalPercent}:</b> ${dataReturned.TotalPercent}%</td>
          </tr>

          <tr>
            ${
            showTries
                ? `<td style="padding: 10px;"><b>${transTry}:</b> ${tryValue}</td>`
                : `<td style="padding: 10px;"></td>`
        }
            <td style="padding: 10px;"><b>${transLevelOfKnowledge}:</b> ${dokValue}</td>
          </tr>
        </table>
    `;

        htmlToAdd += `<div style="background-color: #f7f7f7;">`;

        dataReturned.QuestionQuizList.forEach((question, index) => {
            const qr = question?.QuestionResponses?.[0];

            htmlToAdd += `
        <div style="margin-bottom: 20px; border-bottom: 1px solid #ccc; padding: 20px;">
          <p><b>${index + 1}) ${transQuestion}:</b> <i>${question.Question}</i></p>
          <p>${transStudentResponse}: <i>${qr?.Response ?? ''}</i></p>
          ${qr?.ResponseExtra ? `<p><i>${qr.ResponseExtra}</i></p>` : ''}
          ${autoCorrect ? `
            <p><b style="color: ${qr?.Correct ? '#9bd4ad' : '#b56a62'}">
              ${qr?.Correct ? transCorrect : transIncorrect}
            </b></p>
          ` : ''}
        </div>
      `;
        });

        htmlToAdd += `</div></div>`;
        return htmlToAdd;
    }
}

export default ResultsToHTMLConverter;
