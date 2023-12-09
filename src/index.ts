import { args } from './cliOptions.ts';
import { getClient } from './grahqlClient.ts';
import { getOrganizationQuery } from './queries/organizationQuery.ts';
import { getAllPullRequestReviews, getAllPullRequests } from "./analyzer.ts";
import { getOutput } from "./printer.ts";

async function main() {
	const client = getClient(args.apiToken);

	const from = new Date(args.dateThreshold);

	const { organization } = await getOrganizationQuery(args.repositoryOwner)(
		client,
	);
	
	const allPullRequests = (await getAllPullRequests(
		args.subject,
		organization.id,
		from,
		client,
	));
	const allPullRequestsReviews = (await getAllPullRequestReviews(
		args.subject,
		organization.id,
		from,
		client,
	));

	console.log(getOutput(args.subject, args.repositoryOwner, from, allPullRequests, allPullRequestsReviews));
}

await main();
