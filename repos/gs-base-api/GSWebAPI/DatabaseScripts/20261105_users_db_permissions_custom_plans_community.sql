-- =====================================================================================================
-- Permissions for the Custom Plans and Community modules (GSWebAPI).
-- Target: the USERS database (ConnectionStrings:GenialSkillsUsersAPIDBConnection, e.g. gs_users_db_prod).
-- NOT an EF migration: the users DB is owned by UsersAPI. Review, then run once per environment.
-- Idempotent: safe to run more than once.
--
-- Role 1 = client (AppSettings:ur_role_client).
-- Community forums and posts live in GS Web and use CommunityRead / CommunityHandler (no Forum* permissions).
-- =====================================================================================================

START TRANSACTION;

INSERT INTO ur_permissions (`name`, `desc`, `modified_by`)
SELECT p.`name`, p.`description`, 1
FROM (
    SELECT 'CustomPlanRead'    AS `name`, 'Read custom study plans (GSWebAPI).'                  AS `description`
    UNION ALL SELECT 'CustomPlanHandler', 'Create and modify custom study plans (GSWebAPI).'
    UNION ALL SELECT 'CommunityRead',     'Read community groups, events, resources and forums (GSWebAPI).'
    UNION ALL SELECT 'CommunityHandler',  'Create and modify community groups, events, resources and forums (GSWebAPI).'
) AS p
WHERE NOT EXISTS (SELECT 1 FROM ur_permissions existing WHERE existing.`name` = p.`name`);

INSERT INTO ur_role_permissions (`role_id`, `permission_id`, `modified_by`)
SELECT 1, up.`id`, 1
FROM ur_permissions up
WHERE up.`name` IN (
        'CustomPlanRead', 'CustomPlanHandler',
        'CommunityRead', 'CommunityHandler')
  AND NOT EXISTS (
        SELECT 1 FROM ur_role_permissions rp
        WHERE rp.`role_id` = 1 AND rp.`permission_id` = up.`id`);

-- Check before committing: expect 4 rows.
SELECT up.`name`
FROM ur_role_permissions rp
JOIN ur_permissions up ON up.`id` = rp.`permission_id`
WHERE rp.`role_id` = 1
  AND up.`name` IN ('CustomPlanRead', 'CustomPlanHandler', 'CommunityRead', 'CommunityHandler');

COMMIT;
