using GraphQL;

namespace github_analyzer
{
    internal interface IQuery
    {
        GraphQLRequest CreateRequest();
    }
}
