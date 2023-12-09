import { GraphQLClient } from 'https://deno.land/x/graphql_request@v4.1.0/mod.ts';

export function getClient(apiToken: string) {
	return new GraphQLClient('https://api.github.com/graphql', {
		headers: {
			authorization: `Bearer ${apiToken}`,
			'user-agent': 'github-analyzer',
		},
	});
}
