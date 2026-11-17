import Settings from "../custom-classes/settings";
import { getAuthHeaders } from "./interceptors";
import {
    appendLanguageInstruction
} from "../components/teacher-module/ai-module/components/tools/tool-modal/utils/append-language-instruction";

const DEFAULT_MODEL = "gpt-4o-mini";
// Low temperature keeps the JSON output consistent/well-formed; maxTokens gives
// enough room for several questions with options/pairs without truncating mid-JSON.
const DEFAULT_TEMPERATURE = 0.4;
const DEFAULT_MAX_TOKENS  = 4000;

/**
 * Sends a prompt to /api/generate via SSE streaming and returns the full
 * accumulated response as a string.
 *
 * @param {string} systemPrompt - System prompt
 * @param {string} userMessage  - User message
 * @param {string} [model]
 * @param {number} [temperature] - Output randomness (0 = deterministic, 1 = creative)
 * @param {number} [maxTokens]   - Max tokens the model can generate for the response
 * @returns {Promise<string>}
 */
export async function generateStreamedResponse(
    systemPrompt,
    userMessage,
    model = DEFAULT_MODEL,
    temperature = DEFAULT_TEMPERATURE,
    maxTokens = DEFAULT_MAX_TOKENS,
) {

    systemPrompt = appendLanguageInstruction(systemPrompt);
    userMessage = appendLanguageInstruction(userMessage);

    const response = await fetch(Settings.API_AI_URL + "/aiservicesapi/generate?isFromTeacherApp=true", {
        method:  "POST",
        headers: {
            "Content-Type": "application/json",
            ...getAuthHeaders(),
        },
        body: JSON.stringify({
            model,
            system: systemPrompt,
            user: userMessage,
            temperature,
            maxTokens,
        }),
    });

    if (!response.ok || !response.body) throw new Error(`HTTP ${response.status}`);

    const streamReader    = response.body.getReader();
    const textDecoder     = new TextDecoder();
    let accumulatedText   = "";
    let incompleteLineBuf = "";

    while (true) {
        const { value, done } = await streamReader.read();
        if (done) break;
        incompleteLineBuf += textDecoder.decode(value, { stream: true });
        const rawLines     = incompleteLineBuf.split("\n");
        incompleteLineBuf  = rawLines.pop() ?? "";
        for (const rawLine of rawLines) {
            if (!rawLine.startsWith("data:")) continue;
            const ssePayload = rawLine.slice(5).trim();
            if (ssePayload === "[DONE]") continue;
            try {
                const sseEvent = JSON.parse(ssePayload);
                if (sseEvent.error) throw new Error(sseEvent.error);
                if (sseEvent.delta) accumulatedText += sseEvent.delta;
            } catch { /* ignore non-JSON keep-alives */ }
        }
    }

    return accumulatedText;
}
