# Possible Improvements

* switch web project to .net core

* package up layout and/or layout functionality

* package up authentication
	we could simplify the identity config by
		replace all the endpoints in the config with a single entry to an OIDC metadata endpoint which supplies the endpoints (better, more work, see https://developer.okta.com/blog/2017/07/25/oidc-primer-part-1)
		have a single config containing the endpoints shared between all relying parties, rather than duplicating it in each websites config
	we might be able to optimise the authentication flow by using Hybrid, rather than code and get the tokens at the same time as the code

* security: app service full access to db

* auto-create deadletters queue
