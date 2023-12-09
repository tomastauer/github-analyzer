export type PageInfo = {
	hasNextPage: boolean;
	endCursor: string;
};

export type Connection<T> = {
	nodes: T[];
	pageInfo: PageInfo;
};
