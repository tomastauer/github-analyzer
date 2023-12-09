
import { PullRequest } from './schemas/PullRequest.ts';
import { PullRequestReview } from './schemas/PullRequestReview.ts';

function groupContributions(contributions: (PullRequest | PullRequestReview)[]) {
	return Object.values(contributions.reduce((agg, curr) => {
		(agg[curr.repository.nameWithOwner] = (agg[curr.repository.nameWithOwner] ?? [])).push(curr);

		return agg;
	}, {} as Record<string, (PullRequest | PullRequestReview)[]>)).sort((a, b) => b.length - a.length).map(p => p.sort((a,b) => new Date('createdAt' in b ? b.createdAt : b.submittedAt).getTime() - new Date('createdAt' in a ? a.createdAt : a.submittedAt).getTime())).map(a => ({contribution: {...a[0], time: 'createdAt' in a[0] ? a[0].createdAt : a[0].submittedAt}, total: a.length}));
}

export function getOutput(
	subject: string,
	organization: string,
	from: Date,
	pullRequests: PullRequest[],
	pullRequestReviews: PullRequestReview[],
) {
	const allRepos = [...pullRequestReviews.map(c => c.repository.nameWithOwner), ...pullRequests.map(c=>c.repository.nameWithOwner)];
	const maxRepoLength = Math.max(...allRepos.map(a => a.length));

	const totalWidth = maxRepoLength + 68;
	const numberOfContributions = 'Number of contributions';

	const groupedPullRequests = groupContributions(pullRequests);
	const groupedPullRequestReviews = groupContributions(pullRequestReviews);

	const result = [
			'='.repeat(totalWidth),
			'',
			` Subject: ${subject}\t\t\tOrganization: ${organization}`,
			` From: ${from.toLocaleDateString(undefined, {
				year: "numeric",
				month: "long",
				day: "numeric",
			  })}\t\t\tTo: ${new Date().toLocaleDateString(undefined, {
				year: "numeric",
				month: "long",
				day: "numeric",
			  })}`,
			'',
			'='.repeat(totalWidth),
			'',
			' Pull requests',
			'',
			'-'.repeat(totalWidth),
			` ${'Repository'.padEnd(maxRepoLength, ' ')}\t\t${numberOfContributions}\t\tLast Contribution`,
			'-'.repeat(totalWidth),
			groupedPullRequests.map(g => ` ${g.contribution.repository.nameWithOwner.padEnd(maxRepoLength, ' ')}\t\t${g.total.toString().padStart(numberOfContributions.length,' ')}\t\t${g.contribution.time}`).join('\n'),
			'-'.repeat(totalWidth),
			` ${'Total'.padEnd(maxRepoLength, ' ')}\t\t${groupedPullRequests.reduce((agg, curr) => agg + curr.total, 0).toString().padStart(numberOfContributions.length, ' ')}`,
			'='.repeat(totalWidth),
			'',
			' Pull request reviews',
			'',
			'-'.repeat(totalWidth),
			` ${'Repository'.padEnd(maxRepoLength, ' ')}\t\t${numberOfContributions}\t\tLast Contribution`,
			'-'.repeat(totalWidth),
			groupedPullRequestReviews.map(g => ` ${g.contribution.repository.nameWithOwner.padEnd(maxRepoLength, ' ')}\t\t${g.total.toString().padStart(numberOfContributions.length,' ')}\t\t${g.contribution.time}`).join('\n'),
			'-'.repeat(totalWidth),
			` ${'Total'.padEnd(maxRepoLength, ' ')}\t\t${groupedPullRequestReviews.reduce((agg, curr) => agg + curr.total, 0).toString().padStart(numberOfContributions.length, ' ')}`,
			'='.repeat(totalWidth),
	]

	return result.join('\n');
}
