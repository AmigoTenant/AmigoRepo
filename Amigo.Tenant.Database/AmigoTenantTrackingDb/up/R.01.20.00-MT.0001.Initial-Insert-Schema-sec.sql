--DATABASE OBJECTS CREATION SEC SCHEMA

INSERT [sec].[ClientCorsOrigins] ([Id], [Origin], [Client_Id]) VALUES (3, N'http://127.0.0.1:7070', 1001)
INSERT [sec].[ClientCorsOrigins] ([Id], [Origin], [Client_Id]) VALUES (4, N'http://amigotenant.azurewebsites.net/', 1001)
SET IDENTITY_INSERT [sec].[ClientCorsOrigins] OFF
SET IDENTITY_INSERT [sec].[ClientPostLogoutRedirectUris] ON 

INSERT [sec].[ClientPostLogoutRedirectUris] ([Id], [Uri], [Client_Id]) VALUES (1, N'http://amigotenant.azurewebsites.net:7080', 1001)
INSERT [sec].[ClientPostLogoutRedirectUris] ([Id], [Uri], [Client_Id]) VALUES (2, N'http://127.0.0.1:7070', 1001)
SET IDENTITY_INSERT [sec].[ClientPostLogoutRedirectUris] OFF
SET IDENTITY_INSERT [sec].[ClientRedirectUris] ON 

INSERT [sec].[ClientRedirectUris] ([Id], [Uri], [Client_Id]) VALUES (1004, N'http://amigotenant.azurewebsites.net/', 1001)
INSERT [sec].[ClientRedirectUris] ([Id], [Uri], [Client_Id]) VALUES (1005, N'http://127.0.0.1:7070', 1001)
SET IDENTITY_INSERT [sec].[ClientRedirectUris] OFF
SET IDENTITY_INSERT [sec].[Clients] ON 

INSERT [sec].[Clients] ([Id], [Enabled], [ClientId], [ClientName], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowAccessTokensViaBrowser], [Flow], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessToAllScopes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [AllowAccessToAllGrantTypes]) VALUES (1, 1, N'Amigo.Tenant.Mobile', N'Amigo Tenant Mobile', NULL, NULL, 1, 0, 0, 4, 0, NULL, 0, 0, 0, 300, 7200, 300, 2592000, 28800, 0, 0, 0, 0, 1, 0, 0, 1, 0)
INSERT [sec].[Clients] ([Id], [Enabled], [ClientId], [ClientName], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowAccessTokensViaBrowser], [Flow], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessToAllScopes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [AllowAccessToAllGrantTypes]) VALUES (1001, 1, N'amigo.tenant.web', N'Amigo Tenant', NULL, NULL, 0, 0, 1, 1, 0, NULL, 0, 0, 1, 300, 3600, 300, 1296000, 3600, 0, 0, 0, 0, 1, 0, 0, 1, 0)
INSERT [sec].[Clients] ([Id], [Enabled], [ClientId], [ClientName], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowAccessTokensViaBrowser], [Flow], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessToAllScopes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [AllowAccessToAllGrantTypes]) VALUES (2001, 1, N'amigo.tenant.services', N'Amigo Tenant Services', NULL, NULL, 1, 0, 0, 3, 0, NULL, 0, 0, 0, 300, 36000, 300, 300, 1296000, 0, 0, 0, 0, 1, 0, 0, 1, 0)
SET IDENTITY_INSERT [sec].[Clients] OFF
SET IDENTITY_INSERT [sec].[ClientScopes] ON 

INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (1, N'openid', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (2, N'profile', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (3, N'email', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (4, N'offline_access', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (5, N'roles', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (6, N'XST.Services', 1)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (1001, N'openid', 1001)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (1003, N'profile', 1001)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (1004, N'XST.Services', 1001)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (2001, N'UsersApi', 2001)
INSERT [sec].[ClientScopes] ([Id], [Scope], [Client_Id]) VALUES (2002, N'roles', 1001)
SET IDENTITY_INSERT [sec].[ClientScopes] OFF
SET IDENTITY_INSERT [sec].[ClientSecrets] ON 

INSERT [sec].[ClientSecrets] ([Id], [Value], [Type], [Description], [Expiration], [Client_Id]) VALUES (1001, N'kuZZDRuwjteNiyhfNX8ohaLZZg1L3QMIQUWRzPNBHkdky+PxsbDGHeSKe7E0DD4cqb4lyLVU1ROFvsveDXw8eA==', N'SharedSecret', NULL, NULL, 1)
INSERT [sec].[ClientSecrets] ([Id], [Value], [Type], [Description], [Expiration], [Client_Id]) VALUES (2003, N'f6PgR/71aYvkRGWo2waQBWzABrku8ATy8FS3ogMQZsGEk5rWKaNEGL5dtMnM8UkBSaCHmN6/LSthT/2YnsU7Ug==', N'SharedSecret', NULL, NULL, 2001)
SET IDENTITY_INSERT [sec].[ClientSecrets] OFF
SET IDENTITY_INSERT [sec].[ScopeClaims] ON 

INSERT [sec].[ScopeClaims] ([Id], [Name], [Description], [AlwaysIncludeInIdToken], [Scope_Id]) VALUES (1, N'name', N'Name', 0, 1001)
INSERT [sec].[ScopeClaims] ([Id], [Name], [Description], [AlwaysIncludeInIdToken], [Scope_Id]) VALUES (2, N'sub', N'Subject', 0, 1001)
INSERT [sec].[ScopeClaims] ([Id], [Name], [Description], [AlwaysIncludeInIdToken], [Scope_Id]) VALUES (3, N'email', N'Email', 0, 1001)
INSERT [sec].[ScopeClaims] ([Id], [Name], [Description], [AlwaysIncludeInIdToken], [Scope_Id]) VALUES (4, N'role', N'Role', 0, 6)
INSERT [sec].[ScopeClaims] ([Id], [Name], [Description], [AlwaysIncludeInIdToken], [Scope_Id]) VALUES (5, N'role', N'Role', 0, 1001)
SET IDENTITY_INSERT [sec].[ScopeClaims] OFF
SET IDENTITY_INSERT [sec].[Scopes] ON 

INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (1, 1, N'openid', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (2, 1, N'profile', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (3, 1, N'email', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (4, 1, N'address', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (5, 1, N'offline_access', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (6, 1, N'roles', NULL, NULL, 0, 0, 0, 1, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (7, 1, N'all_claims', NULL, NULL, 0, 0, 0, 0, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (1001, 1, N'XST.Services', N'XST.Services', N'XST.Services', 0, 0, 1, 1, NULL, 0, 0)
INSERT [sec].[Scopes] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [Type], [IncludeAllClaimsForUser], [ClaimsRule], [ShowInDiscoveryDocument], [AllowUnrestrictedIntrospection]) VALUES (1003, 1, N'UsersApi', NULL, NULL, 0, 0, 1, 0, NULL, 0, 0)
SET IDENTITY_INSERT [sec].[Scopes] OFF
SET IDENTITY_INSERT [sec].[User] ON 

INSERT [sec].[User] ([UserId], [UserName], [FirstName], [LastName], [ProfilePictureUrl], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [RowStatus]) VALUES (3015, N'Root', N'Amigo', N'Tenant', NULL, N'yhairt@hotmail.com', 0, N'ACGmmuNb7m5Ftyr0SccqKCplcfbrpFYLfMtpqOJpHMe8SX50CMC1iGLzjpLIx5yZtA==', N'af695604-6c38-44ed-8dd6-aca1b09c9cf8', NULL, 0, 0, CAST(N'2017-10-17T11:58:14.663' AS DateTime), 0, 0, 1)
INSERT [sec].[User] ([UserId], [UserName], [FirstName], [LastName], [ProfilePictureUrl], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [RowStatus]) VALUES (3019, N'administrator', N'AmigoTenant', N'Admin', NULL, N'yhairt@hotmail.com', 0, N'AO/n1Lk5/tzsGfm0Ax7bNXrjr6hJprkoZiTU2HPCsgv/BSWdkYIIRGr0j+iPBYmQPg==', N'6c9a8518-c840-462f-94bc-01c750a56e83', NULL, 0, 0, NULL, 0, 0, 0)
INSERT [sec].[User] ([UserId], [UserName], [FirstName], [LastName], [ProfilePictureUrl], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [RowStatus]) VALUES (3100, N'EDGARR', N'Edgar', N'Romero', NULL, N'ERomero@amigo.com', 0, N'AMlI7MkqpuvRvw6LG2goyVtGFP67NKC198ZwbDWB2O+fH5zD17vCO4YUOtIPeLUAIw==', N'36815459-5571-4359-ad98-05738cf14337', NULL, 0, 0, NULL, 0, 0, 1)
INSERT [sec].[User] ([UserId], [UserName], [FirstName], [LastName], [ProfilePictureUrl], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [RowStatus]) VALUES (3101, N'MARIAVE', N'Maria', N'Veramatos', NULL, N'ma.ver@gmail.com', 0, N'AAUljygvfbnRKT09o5PsN4JryIY8ckfEK3ZAzDhQij5vTdTXIoMFckjttTe2VlUESw==', N'a158c768-b4e9-41a6-b5ec-6bff208e17d3', NULL, 0, 0, NULL, 0, 0, 1)
INSERT [sec].[User] ([UserId], [UserName], [FirstName], [LastName], [ProfilePictureUrl], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [RowStatus]) VALUES (3102, N'YANIT', N'Yanitza', N'Gomez', NULL, N'ya.go@gmail.com', 0, N'AObw+b7E+3kaDVeLqhffhDLmbNmSwF27k3PIHl0zWqg6dO+DOrDx2pIkPWJmKEYr1w==', N'600cfc7c-4725-43de-b933-29d66e0af334', NULL, 0, 0, NULL, 0, 0, 1)
SET IDENTITY_INSERT [sec].[User] OFF
SET IDENTITY_INSERT [sec].[UserClaim] ON 

INSERT [sec].[UserClaim] ([UserClaimId], [UserId], [ClaimType], [ClaimValue]) VALUES (2065, 3019, N'role', N'ADMINISTRATOR')
INSERT [sec].[UserClaim] ([UserClaimId], [UserId], [ClaimType], [ClaimValue]) VALUES (2276, 3015, N'role', N'ROOT')
INSERT [sec].[UserClaim] ([UserClaimId], [UserId], [ClaimType], [ClaimValue]) VALUES (2443, 3100, N'role', N'ROOT')
INSERT [sec].[UserClaim] ([UserClaimId], [UserId], [ClaimType], [ClaimValue]) VALUES (2444, 3101, N'role', N'ROOT')
INSERT [sec].[UserClaim] ([UserClaimId], [UserId], [ClaimType], [ClaimValue]) VALUES (2445, 3102, N'role', N'ROOT')
SET IDENTITY_INSERT [sec].[UserClaim] OFF
