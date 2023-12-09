# github-analyzer

Utilizes Github GraphQL API to get simple statistics about author contributions for given organization.

## Usage

```
USAGE:
        deno run --allow-net [OPTIONS]

OPTIONS:
        -s <STRING> (required)  Github login of the subject
        -r <STRING> (required)  Github repository owner (analytics will be done only against the owner's repos)
        -t <STRING> (required)  Github API token
        -d <STRING> (required)  Date threshold, PRs older than this will be ignored. Use format yyyy-mm-dd
```
