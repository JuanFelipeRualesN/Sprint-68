using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Azure.Identity;

namespace GSAIServicesAPI.Services.Foundry
{
    /// <summary>
    /// The Foundry project with the RAG resources (Foundry:RAGProjectEndpoint), shared by the modules that use it,
    /// like the plan generation and the chatbot. It is apart from the project of the agent (Foundry:ProjectEndpoint).
    /// </summary>
    /// <remarks>
    /// The client is created the first time it is used, so an environment without Foundry:RAGProjectEndpoint
    /// only fails the requests that need this project. It works with the app's identity, which needs the
    /// "Azure AI User" role in the project.
    /// </remarks>
    public sealed class RagProjectClient
    {
        private const string EndpointKey = "Foundry:RAGProjectEndpoint";

        private readonly Lazy<AIProjectClient> _client;

        public RagProjectClient(IConfiguration configuration)
        {
            _client = new Lazy<AIProjectClient>(() =>
            {
                var endpoint = configuration[EndpointKey];
                if (string.IsNullOrWhiteSpace(endpoint))
                    throw new InvalidOperationException($"Missing config: {EndpointKey}");

                return new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential());
            });
        }

        /// <summary>
        /// The project's client, for what the helpers here do not cover (agents, files, conversations...).
        /// </summary>
        public AIProjectClient Project => _client.Value;

        /// <summary>
        /// To get the Responses client of one of the project's model deployments, without any agent.
        /// </summary>
        /// <param name="deploymentName">The model deployment's name.</param>
        /// <returns>Returns the client bound to the deployment.</returns>
        public ProjectResponsesClient GetResponsesClientForModel(string deploymentName)
            => _client.Value.ProjectOpenAIClient.GetProjectResponsesClientForModel(deploymentName);
    }
}
