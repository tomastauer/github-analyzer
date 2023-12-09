import { Connection } from './Connection.ts';
import { PullRequest } from './PullRequest.ts';
import { PullRequestReview } from './PullRequestReview.ts';

type PullRequestContribution = {
	pullRequest: PullRequest;
};

type PullRequestReviewContribution = {
	pullRequestReview: PullRequestReview;
};

export type User = {
	contributionsCollection: {
		pullRequestContributions: Connection<PullRequestContribution>;
		pullRequestReviewContributions: Connection<
			PullRequestReviewContribution
		>;
	};
};
