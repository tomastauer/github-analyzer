import { GraphQLClient } from 'https://deno.land/x/graphql_request@v4.1.0/mod.ts';
import { getUserPullRequestContributionsQuery } from './queries/userPullRequestContributionsQuery.ts';
import { PageInfo } from './schemas/Connection.ts';
import { PullRequest } from './schemas/PullRequest.ts';
import { getUserPullRequestReviewContributionsQuery } from './queries/userPullRequestReviewContributionsQuery.ts';
import { PullRequestReview } from './schemas/PullRequestReview.ts';

export async function getAllPullRequests(
	subject: string,
	organizationId: string,
	threshold: Date,
	client: GraphQLClient,
) {
	const result: PullRequest[] = [];

	let hasNextPage = true;
	let cursor: string | null = null;
	do {
		const { user } = await getUserPullRequestContributionsQuery(
			subject,
			organizationId,
			threshold,
			new Date(),
			cursor,
		)(client);

		result.push(
			...user.contributionsCollection.pullRequestContributions.nodes.map(
				(n) => n.pullRequest,
			),
		);
		const pageInfo: PageInfo =
			user.contributionsCollection.pullRequestContributions.pageInfo;

		cursor = pageInfo.endCursor;
		hasNextPage = pageInfo.hasNextPage;
	} while (hasNextPage);

	return result.filter((r) => r.merged);
}

export async function getAllPullRequestReviews(
	subject: string,
	organizationId: string,
	threshold: Date,
	client: GraphQLClient,
) {
	const result: PullRequestReview[] = [];

	let hasNextPage = true;
	let cursor: string | null = null;
	do {
		const { user } = await getUserPullRequestReviewContributionsQuery(
			subject,
			organizationId,
			threshold,
			new Date(),
			cursor,
		)(client);

		result.push(
			...user.contributionsCollection.pullRequestReviewContributions.nodes
				.map((n) => n.pullRequestReview),
		);
		const pageInfo: PageInfo =
			user.contributionsCollection.pullRequestReviewContributions
				.pageInfo;

		cursor = pageInfo.endCursor;
		hasNextPage = pageInfo.hasNextPage;
	} while (hasNextPage);

	return result.filter((r) => r.state === 'APPROVED');
}
