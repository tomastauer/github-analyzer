import {
	gql,
	GraphQLClient,
} from 'https://deno.land/x/graphql_request@v4.1.0/mod.ts';
import { OrganizationResult } from '../schemas/OrganizationResult.ts';

export function getOrganizationQuery(organization: string) {
	const query = gql`
		query Organization($login: String!) {
			organization(login: $login) {
				id
			}
		}
		`;

	return (grapQLClient: GraphQLClient) =>
		grapQLClient.request<OrganizationResult>(query, {
			login: organization,
		});
}
