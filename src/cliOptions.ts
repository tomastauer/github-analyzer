import { Command, string } from 'https://deno.land/x/clay/mod.ts';

export const args = new Command('github-analyzer')
	.required(string, 'subject', {
		flags: ['s'],
		description: 'Github login of the subject',
	})
	.required(string, 'repositoryOwner', {
		flags: ['r'],
		description:
			'Github repository owner (analytics will be done only against the owner\'s repos)',
	})
	.required(string, 'apiToken', {
		flags: ['t'],
		description: 'Github API token',
	})
	.required(string, 'dateThreshold', {
		flags: ['d'],
		description:
			'Date threshold, PRs older than this will be ignored. Use format yyyy-mm-dd',
	}).run();
