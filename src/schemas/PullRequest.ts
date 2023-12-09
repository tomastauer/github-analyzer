import { Repository } from './Repository.ts';

export type PullRequest = {
	merged: boolean;
	repository: Repository;
	createdAt: string;
};
