import {
	gql,
	GraphQLClient,
} from 'https://deno.land/x/graphql_request@v4.1.0/mod.ts';
import { UserResult } from '../schemas/UserResult.ts';

export function getUserPullRequestContributionsQuery(
	subject: string,
	organizationId: string,
	from: Date,
	to: Date,
	after: string | null,
) {
	const query = gql`
		query User($login: String!, $organizationID: ID!, $from: DateTime!, $to: DateTime!, $after: String) {
			user(login: $login) {
				contributionsCollection(organizationID: $organizationID, from: $from, to: $to) {
					pullRequestContributions(first: 100, after: $after) {
						nodes {
							pullRequest {
               					merged
                				repository {
									nameWithOwner
								}
								createdAt
            				}
						}
						pageInfo {
    						hasNextPage
    						endCursor
						}
					}
				}
			}
		}
		`;

	return (grapQLClient: GraphQLClient) =>
		grapQLClient.request<UserResult>(query, {
			login: subject,
			organizationID: organizationId,
			from: from.toISOString(),
			to: to.toISOString(),
			after: after,
		});
}
