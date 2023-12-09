import { Repository } from './Repository.ts';

export type PullRequestReview = {
	state: 'APPROVED' | 'COMMENTED';
	repository: Repository;
	submittedAt: string;
};
