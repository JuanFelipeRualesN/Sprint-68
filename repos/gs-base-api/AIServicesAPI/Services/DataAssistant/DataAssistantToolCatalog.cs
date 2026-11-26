using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// One tool of the data assistant: a usage insights query that the model may ask for.
    /// </summary>
    /// <param name="Name">Name the model uses to call the tool (snake_case).</param>
    /// <param name="QueryName">Route name of the query in the data analysis module (kebab-case).</param>
    /// <param name="Description">What the tool returns and when to use it.</param>
    /// <param name="ParametersSchema">JSON schema of the arguments.</param>
    /// <param name="ParameterNames">Names of the declared arguments; anything else the model sends is dropped.</param>
    public sealed record DataAssistantToolDefinition(string Name, string QueryName, string Description, BinaryData ParametersSchema, IReadOnlySet<string> ParameterNames);

    /// <summary>
    /// Loads the tool definitions and the system instructions from the embedded resources (Resources/DataAssistant) and turns them into the
    /// tools of the Chat Completions request. The tool names and parameters are the ones of the API contract, so the catalog is the single source of the tools.
    /// </summary>
    public sealed class DataAssistantToolCatalog
    {
        private const string ToolDefinitionsResource = "DataAssistant/tool-definitions.json";
        private const string InstructionsResource = "DataAssistant/system-instructions.txt";
        private const string KnowledgeBaseResource = "DataAssistant/dashboard-knowledge.md";
        private readonly Dictionary<string, DataAssistantToolDefinition> _toolsByName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantToolCatalog"/> class reading the embedded resources.
        /// </summary>
        public DataAssistantToolCatalog()
            : this(ReadResource(ToolDefinitionsResource), ReadResource(InstructionsResource), ReadResource(KnowledgeBaseResource))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantToolCatalog"/> class from the given texts, without a knowledge base.
        /// </summary>
        /// <param name="toolDefinitionsJson">The JSON array of tool definitions.</param>
        /// <param name="instructionsTemplate">The system instructions with their {placeholders}.</param>
        public DataAssistantToolCatalog(string toolDefinitionsJson, string instructionsTemplate)
            : this(toolDefinitionsJson, instructionsTemplate, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantToolCatalog"/> class from the given texts.
        /// </summary>
        /// <param name="toolDefinitionsJson">The JSON array of tool definitions.</param>
        /// <param name="instructionsTemplate">The system instructions with their {placeholders}.</param>
        /// <param name="knowledgeBase">The knowledge base about the dashboard (plain text); it has no placeholders.</param>
        public DataAssistantToolCatalog(string toolDefinitionsJson, string instructionsTemplate, string knowledgeBase)
        {
            InstructionsTemplate = instructionsTemplate.Trim();
            KnowledgeBase = knowledgeBase.Trim();
            var tools = new List<DataAssistantToolDefinition>();
            foreach (var definition in JsonNode.Parse(toolDefinitionsJson)!.AsArray())
            {
                var parameters = definition!["Parameters"]!.AsObject();
                var schema = new JsonObject
                {
                    ["type"] = "object",
                    ["properties"] = parameters.DeepClone(),
                    ["required"] = definition["Required"]!.DeepClone(),
                };
                tools.Add(new DataAssistantToolDefinition(
                    definition["Name"]!.GetValue<string>(),
                    definition["Route"]!.GetValue<string>(),
                    definition["Description"]!.GetValue<string>(),
                    BinaryData.FromString(schema.ToJsonString()),
                    parameters.Select(parameter => parameter.Key).ToHashSet(StringComparer.Ordinal)));
            }

            Tools = tools;
            _toolsByName = tools.ToDictionary(tool => tool.Name, StringComparer.Ordinal);
        }

        /// <summary>
        /// Gets the system instructions with their {placeholders} still to be filled.
        /// </summary>
        public string InstructionsTemplate { get; }

        /// <summary>
        /// Gets the knowledge base that explains the dashboard (pages, terms and calculations); empty when there is none. It holds no data.
        /// </summary>
        public string KnowledgeBase { get; }

        /// <summary>
        /// Gets the tools of the catalog.
        /// </summary>
        public IReadOnlyList<DataAssistantToolDefinition> Tools { get; }

        /// <summary>
        /// Finds a tool by the name the model used.
        /// </summary>
        /// <param name="name">The tool name.</param>
        /// <param name="tool">The tool, when found.</param>
        /// <returns>Returns true when the tool exists.</returns>
        public bool TryGetTool(string name, out DataAssistantToolDefinition tool) => _toolsByName.TryGetValue(name, out tool!);

        /// <summary>
        /// Builds the "tools" array of a Chat Completions request: one function tool per catalog tool, with its JSON schema.
        /// </summary>
        /// <returns>Returns a new array on every call (a node can only have one parent).</returns>
        public JsonArray BuildToolsJson()
        {
            var tools = new JsonArray();
            foreach (var tool in Tools)
            {
                tools.Add(new JsonObject
                {
                    ["type"] = "function",
                    ["function"] = new JsonObject
                    {
                        ["name"] = tool.Name,
                        ["description"] = tool.Description,
                        ["parameters"] = JsonNode.Parse(tool.ParametersSchema.ToString()),
                    },
                });
            }

            return tools;
        }

        private static string ReadResource(string name)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)
                ?? throw new InvalidOperationException($"Embedded resource not found: {name}");
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
