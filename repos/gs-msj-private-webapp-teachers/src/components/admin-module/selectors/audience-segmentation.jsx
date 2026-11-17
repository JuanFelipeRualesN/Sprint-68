import React, { useState, useEffect, useRef } from 'react';
import { Button } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import { getRoles, getUsersAdvancedAdmin, getUserEmailsBySchools } from "../../../api/users";
import { getRegions, getEntitiesSchool, getOreSchoolsIds, getSchoolEntitiesByIds } from "../../../api/schools";
import Select from '../../shared-module-components/selector/common-selector';

const mapUser = (user) => ({
    value: user.UserId,
    label: `${user.GivenName || ''} ${user.Surname || ''}`.trim(),
});

const mergeOptions = (base, extra) => {
    const merged = [...base];
    extra.forEach(item => {
        if (!merged.some(option => option.value === item.value)) {
            merged.push(item);
        }
    });
    return merged;
};

// Safety backstop only — not expected to be hit (5000 schools). Unlike Users API's
// GetUsersForEvents, this endpoint's PageTotals is accurate, so it's safe to trust for the
// real page count instead of looping blindly.
const MAX_SCHOOL_PAGES = 50;
const SCHOOL_PAGE_SIZE = 100;

// Generous limiter for getUserEmailsBySchools — that endpoint has no per-call cap, unlike for-admin.
const SCHOOL_USERS_LIMITER = 5000;

// for-admin caps its own response at ~200 rows regardless of Limiter, so name lookups for a
// school's full user list must go through it in chunks this size.
const NAME_LOOKUP_CHUNK_SIZE = 200;

const chunkArray = (items, size) => {
    const chunks = [];
    for (let i = 0; i < items.length; i += size) {
        chunks.push(items.slice(i, i + size));
    }
    return chunks;
};

/**
 * Handles the "Audience segmentation" section of the event form: Role, Region, School and
 * Specific user criteria. Each criterion is toggled independently and persists exactly as
 * event_regions / event_schools / event_roles / event_users already do.
 *
 * Role and School criteria define the audience directly (event_roles / event_schools).
 * The specific-user flow has its own, separate Region/School/Role filter cascade — used only
 * to narrow the candidate search, never persisted on its own.
 */
const AudienceSegmentation = ({
    eventRoleModelList = [],
    eventSchoolModelList = [],
    eventRegionModelList = [],
    eventUserModelList = [],
    updateParent,
}) => {
    const { t } = useTranslation('events');
    const [roleOptions, setRoleOptions] = useState([]);
    const [regionOptions, setRegionOptions] = useState([]);
    const [schoolOptions, setSchoolOptions] = useState([]);
    const [userOptions, setUserOptions] = useState([]);

    const [roleEnabled, setRoleEnabled] = useState(false);
    const [regionEnabled, setRegionEnabled] = useState(false);
    const [schoolEnabled, setSchoolEnabled] = useState(false);
    const [userEnabled, setUserEnabled] = useState(false);

    // Audience-defining selections (persisted as event_roles / event_schools / event_regions).
    const [selectedAudienceRoles, setSelectedAudienceRoles] = useState([]);
    const [selectedRegions, setSelectedRegions] = useState([]);
    const [selectedAudienceSchools, setSelectedAudienceSchools] = useState([]);

    // School criterion's own, optional override — completely independent state from the
    // specific-user flow. When non-empty, these users replace "all users of the selected schools".
    const [selectedSchoolUsers, setSelectedSchoolUsers] = useState([]);
    const [schoolUserOptions, setSchoolUserOptions] = useState([]);

    // Specific-user flow's own filter, used only to narrow the user search below. Never
    // persisted on its own — completely independent from selectedAudienceSchools/selectedAudienceRoles.
    // Role is single-select here (matches the mockup's cascade), including a sentinel "all
    // roles" option (empty value) — so there's never an ambiguous multi-role search to block.
    const [selectedUserFilterRegion, setSelectedUserFilterRegion] = useState(null);
    const [userFilterSchoolOptions, setUserFilterSchoolOptions] = useState([]);
    const [selectedUserFilterSchools, setSelectedUserFilterSchools] = useState([]);
    const [selectedUserFilterRole, setSelectedUserFilterRole] = useState(null);
    const [selectedUsers, setSelectedUsers] = useState([]);

    const [eventRoleIds, setEventRoleIds] = useState([]);
    const [eventRegionIds, setEventRegionIds] = useState([]);
    const [eventSchoolIds, setEventSchoolIds] = useState([]);
    const [eventUserIds, setEventUserIds] = useState([]);

    const hasUserFilter = selectedUserFilterSchools.length > 0 || Boolean(selectedUserFilterRole);
    const roleOptionsWithAll = [{ value: '', label: t('audience-segmentation.all-roles-option') }, ...roleOptions];

    // Load the small, global catalogs once.
    useEffect(() => {
        const loadCatalogs = async () => {
            try {
                const res = await getRoles();
                setRoleOptions(res.data.map(role => ({ value: role.Id, label: role.Desc })));
            } catch { }
            try {
                const res = await getRegions();
                setRegionOptions(res.data.map(region => ({ value: region.Id, label: region.Name })));
            } catch { }
        };
        loadCatalogs();
    }, []);

    // School criterion's catalog: a UX-only narrowing, not an audience-combination change
    // (Region and School still combine by union at the audience level, same as always) — with
    // no region selected, show every school; with one or more selected, show only their schools
    // (unioned across all of them, same SchoolsAPI resolution already used by the specific-user
    // cascade).
    useEffect(() => {
        const loadSchools = async () => {
            if (selectedRegions.length === 0) {
                try {
                    const firstPage = await getEntitiesSchool(null, null, null, null, null, 0, SCHOOL_PAGE_SIZE);
                    const totalPages = Math.min(firstPage.data.PageTotals || 0, MAX_SCHOOL_PAGES);
                    const remainingPages = await Promise.all(
                        Array.from({ length: Math.max(totalPages - 1, 0) }, (_, index) =>
                            getEntitiesSchool(null, null, null, null, null, index + 1, SCHOOL_PAGE_SIZE))
                    );
                    const allSchools = [firstPage, ...remainingPages]
                        .flatMap(res => res.data.SchoolEntities || []);
                    setSchoolOptions(allSchools.map(school => ({ value: school.EntityId, label: school.EntityName })));
                } catch {
                    setSchoolOptions([]);
                }
                return;
            }

            try {
                const schoolIdsPerRegion = await Promise.all(
                    selectedRegions.map(region => getOreSchoolsIds(region.value))
                );
                const schoolIds = [...new Set(schoolIdsPerRegion.flatMap(res => res.data || []))];
                if (schoolIds.length === 0) {
                    setSchoolOptions([]);
                    return;
                }
                const entitiesRes = await getSchoolEntitiesByIds(schoolIds);
                setSchoolOptions(entitiesRes.data.map(entity => ({ value: entity.Id, label: entity.Name })));
            } catch {
                setSchoolOptions([]);
            }
        };
        loadSchools();
    }, [selectedRegions]);

    // Reconcile incoming event data (edit mode): pull the assigned ids and auto-enable
    // whichever criteria the event already uses.
    useEffect(() => {
        setEventRoleIds(eventRoleModelList.map(role => role.RoleId));
        setEventRegionIds(eventRegionModelList.map(region => region.RegionId));
        setEventSchoolIds(eventSchoolModelList.map(school => school.SchoolIdentifier));
        setEventUserIds(eventUserModelList.map(user => user.UserId));

        if (eventRoleModelList.length > 0) setRoleEnabled(true);
        if (eventRegionModelList.length > 0) setRegionEnabled(true);
        if (eventSchoolModelList.length > 0) setSchoolEnabled(true);
        if (eventUserModelList.length > 0) setUserEnabled(true);
    }, [eventRoleModelList, eventRegionModelList, eventSchoolModelList, eventUserModelList]);

    useEffect(() => {
        setSelectedAudienceRoles(roleOptions.filter(option => eventRoleIds.includes(option.value)));
    }, [roleOptions, eventRoleIds]);

    useEffect(() => {
        setSelectedRegions(regionOptions.filter(option => eventRegionIds.includes(option.value)));
    }, [regionOptions, eventRegionIds]);

    useEffect(() => {
        setSelectedAudienceSchools(schoolOptions.filter(option => eventSchoolIds.includes(option.value)));
    }, [schoolOptions, eventSchoolIds]);

    // School criterion's own optional override: load the candidate users belonging to the
    // schools selected above. Two steps, since no single endpoint gives both full coverage
    // and names: (1) getUserEmailsBySchools for every id+email, (2) getUsersAdvancedAdmin,
    // chunked, to resolve names. No state or cascade is shared with the specific-user flow.
    useEffect(() => {
        if (!schoolEnabled || selectedAudienceSchools.length === 0) {
            setSchoolUserOptions([]);
            return;
        }

        const loadSchoolUsers = async () => {
            try {
                const schoolIdsFilter = selectedAudienceSchools.map(school => school.value);
                const emailsRes = await getUserEmailsBySchools(schoolIdsFilter, SCHOOL_USERS_LIMITER);
                const idsAndEmails = emailsRes.data || [];
                if (idsAndEmails.length === 0) {
                    setSchoolUserOptions([]);
                    return;
                }

                const userIds = idsAndEmails.map(user => user.UserId);
                const nameChunkResponses = await Promise.all(
                    chunkArray(userIds, NAME_LOOKUP_CHUNK_SIZE).map(chunk =>
                        getUsersAdvancedAdmin(chunk, null, null, null, null, null, null,
                            null, null, 0, chunk.length, false, null))
                );
                const namesByUserId = new Map(
                    nameChunkResponses.flatMap(res => res.data.UserInfo || [])
                        .map(user => [user.UserId, user])
                );

                setSchoolUserOptions(idsAndEmails.map(user => {
                    const resolved = namesByUserId.get(user.UserId);
                    return resolved ? mapUser(resolved) : { value: user.UserId, label: user.Email };
                }));
            } catch { }
        };
        loadSchoolUsers();
    }, [schoolEnabled, selectedAudienceSchools]);

    // Resolve the labels of already-assigned specific users in a single batched call. This is
    // independent of the School/Role filter below (which starts empty in edit mode too — it
    // was never persisted, so there is nothing to reconstruct for it).
    useEffect(() => {
        if (eventUserIds.length === 0) return;

        const resolveSelectedUsers = async () => {
            try {
                const res = await getUsersAdvancedAdmin(
                    eventUserIds, null, null, null, null, null, null,
                    null, null, 0, eventUserIds.length, false, null
                );
                const resolved = (res.data.UserInfo || []).map(mapUser);
                setSelectedUsers(resolved);
                setUserOptions(prev => mergeOptions(prev, resolved));
            } catch { }
        };
        resolveSelectedUsers();
    }, [eventUserIds]);

    // Specific-user cascade, step 1→2: once a Region is picked, resolve its school ids
    // (SchoolsAPI ORE endpoint) and then their names, to populate the School options below.
    useEffect(() => {
        setSelectedUserFilterSchools([]);

        if (!selectedUserFilterRegion) {
            setUserFilterSchoolOptions([]);
            return;
        }

        const loadRegionSchools = async () => {
            try {
                const idsRes = await getOreSchoolsIds(selectedUserFilterRegion.value);
                const schoolIds = idsRes.data || [];
                if (schoolIds.length === 0) {
                    setUserFilterSchoolOptions([]);
                    return;
                }
                const entitiesRes = await getSchoolEntitiesByIds(schoolIds);
                setUserFilterSchoolOptions(entitiesRes.data.map(entity => ({ value: entity.Id, label: entity.Name })));
            } catch {
                setUserFilterSchoolOptions([]);
            }
        };
        loadRegionSchools();
    }, [selectedUserFilterRegion]);

    // Load the candidate list for the specific-user step, filtered by this panel's own
    // School/Role selection. Runs when the panel is activated and whenever those two
    // filters change — nothing else is in this dependency array, so it never re-fires
    // on unrelated renders. Never queries with an empty filter (no school and no role
    // selected yet) — that would bring back an unfiltered page of users for no reason.
    //
    // Limit is generous (5000) rather than a small display page: this list doubles as the
    // full audience when no individual user is picked below (mockup: "sin selección se
    // incluye a todos del rol en esa escuela"). It's safe at this size because the cascade
    // always requires a Region (and usually a School) before Role unlocks, so the matching
    // population is bounded to that scope — never the district-wide scale that required the
    // single-large-call fix in GetUsersForEvents.
    useEffect(() => {
        if (!userEnabled || !hasUserFilter) {
            setUserOptions([]);
            return;
        }

        const loadCandidateUsers = async () => {
            try {
                const roleIdFilter = selectedUserFilterRole?.value || null;
                const schoolIdsFilter = selectedUserFilterSchools.map(school => school.value);
                const res = await getUsersAdvancedAdmin(
                    null, null, null, null, null, null, roleIdFilter,
                    null, null, 0, 5000, false, schoolIdsFilter.length > 0 ? schoolIdsFilter : null
                );
                setUserOptions((res.data.UserInfo || []).map(mapUser));
            } catch { }
        };
        loadCandidateUsers();
    }, [userEnabled, selectedUserFilterSchools, selectedUserFilterRole, hasUserFilter]);

    // Clear the specific-user selection whenever the School/Role filter genuinely changes
    // (not on mount/edit-mode reconciliation, which sets these refs to the same initial
    // array reference — only a real setState from the selects below counts as a change).
    const prevFilterSchoolsRef = useRef(selectedUserFilterSchools);
    const prevFilterRoleRef = useRef(selectedUserFilterRole);
    useEffect(() => {
        const schoolsChanged = prevFilterSchoolsRef.current !== selectedUserFilterSchools;
        const roleChanged = prevFilterRoleRef.current !== selectedUserFilterRole;
        prevFilterSchoolsRef.current = selectedUserFilterSchools;
        prevFilterRoleRef.current = selectedUserFilterRole;

        if (schoolsChanged || roleChanged) {
            setSelectedUsers([]);
        }
    }, [selectedUserFilterSchools, selectedUserFilterRole]);

    useEffect(() => {
        // School criterion override: when specific users are picked for the selected schools,
        // only they should receive the event — so the schools themselves are left out of the
        // payload and only those users are sent (merged with the specific-user flow's own
        // selection, if active).
        const schoolOverrideActive = schoolEnabled && selectedSchoolUsers.length > 0;

        // Specific-user cascade: if the admin navigated the Region/School/Role filter but never
        // picked an individual, the whole matching candidate pool becomes the audience (mockup:
        // "sin selección se incluye a todos del rol en esa escuela") — not an empty result.
        const cascadeUsers = userEnabled
            ? (selectedUsers.length > 0 ? selectedUsers : (hasUserFilter ? userOptions : []))
            : [];

        const mergedUserModelList = [...cascadeUsers, ...(schoolOverrideActive ? selectedSchoolUsers : [])]
            .filter((user, index, all) => all.findIndex(other => other.value === user.value) === index)
            .map(user => ({ UserId: user.value }));

        updateParent({
            EventRoleModelList: roleEnabled ? selectedAudienceRoles.map(role => ({ RoleId: role.value })) : [],
            EventSchoolModelList: schoolEnabled && !schoolOverrideActive ? selectedAudienceSchools.map(school => ({ SchoolIdentifier: school.value })) : [],
            EventRegionModelList: regionEnabled ? selectedRegions.map(region => ({ RegionId: region.value })) : [],
            EventUserModelList: mergedUserModelList,
        });
    }, [roleEnabled, schoolEnabled, regionEnabled, userEnabled, selectedAudienceRoles, selectedAudienceSchools, selectedRegions, selectedUsers, selectedSchoolUsers, userOptions, hasUserFilter]);

    // Cascade resets: changing an upper step must clear everything below it in the specific-user
    // flow, so the cascade can never show a stale Role/User left over from a previous Region/School.
    const handleUserFilterRegionChange = (region) => {
        setSelectedUserFilterRegion(region);
        setSelectedUserFilterRole(null);
        setSelectedUsers([]);
    };

    const handleUserFilterSchoolsChange = (schools) => {
        setSelectedUserFilterSchools(schools);
        setSelectedUserFilterRole(null);
        setSelectedUsers([]);
    };

    // School criterion's own reset: changing the selected schools invalidates any previously
    // chosen override users (they may no longer belong to the new set of schools).
    const handleAudienceSchoolsChange = (schools) => {
        setSelectedAudienceSchools(schools);
        setSelectedSchoolUsers([]);
    };

    // Region criterion's own reset: changing the selected regions invalidates the previously
    // chosen schools and, transitively, any school-scoped override users — same cascade-reset
    // pattern as handleAudienceSchoolsChange/handleUserFilterRegionChange, independent state.
    const handleAudienceRegionsChange = (regions) => {
        setSelectedRegions(regions);
        setSelectedAudienceSchools([]);
        setSelectedSchoolUsers([]);
    };

    // Each segmentation type toggles independently — activating or deactivating one never
    // touches the others, so any combination of Role/Region/School/User can stay active at
    // once. Deactivating a type still clears only its own selection state, same as before.
    const toggleRoleSegmentation = () => {
        if (roleEnabled) setSelectedAudienceRoles([]);
        setRoleEnabled(!roleEnabled);
    };

    const toggleRegionSegmentation = () => {
        if (regionEnabled) setSelectedRegions([]);
        setRegionEnabled(!regionEnabled);
    };

    const toggleSchoolSegmentation = () => {
        if (schoolEnabled) {
            setSelectedAudienceSchools([]);
            setSelectedSchoolUsers([]);
        }
        setSchoolEnabled(!schoolEnabled);
    };

    const toggleUserSegmentation = () => {
        if (userEnabled) {
            setSelectedUserFilterRegion(null);
            setSelectedUserFilterSchools([]);
            setSelectedUserFilterRole(null);
            setSelectedUsers([]);
        }
        setUserEnabled(!userEnabled);
    };

    const criterionPill = (label, active, onClick) => (
        <Button
            type="button"
            className={active ? 'rounded-pill' : 'rounded-pill btn-grey text-white'}
            onClick={onClick}
            style={active ? { backgroundColor: '#28a745', borderColor: '#28a745', color: '#FFF' } : undefined}
        >
            {active ? '✓ ' : ''}{label}
        </Button>
    );

    return (
        <>
            <div className="d-flex flex-wrap" style={{ gap: '0.75em', marginBottom: '1.5em' }}>
                {criterionPill(t('audience-segmentation.pill-role'), roleEnabled, toggleRoleSegmentation)}
                {criterionPill(t('audience-segmentation.pill-region'), regionEnabled, toggleRegionSegmentation)}
                {criterionPill(t('audience-segmentation.pill-school'), schoolEnabled, toggleSchoolSegmentation)}
                {criterionPill(t('audience-segmentation.pill-user'), userEnabled, toggleUserSegmentation)}
            </div>

            {roleEnabled && (
                <div className="form-group-fields mb-4">
                    <div className="label" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        {t('audience-segmentation.roles-label')}
                        <Button color="link" size="sm" className="p-0" onClick={() => setSelectedAudienceRoles(roleOptions)}>
                            {t('audience-segmentation.select-all-roles')}
                        </Button>
                    </div>
                    <Select
                        isMulti
                        options={roleOptions}
                        value={selectedAudienceRoles}
                        onChange={setSelectedAudienceRoles}
                        placeholder={t('audience-segmentation.roles-placeholder')}
                        noOptionsMessage={() => t('audience-segmentation.no-options')}
                    />
                </div>
            )}

            {regionEnabled && (
                <div className="form-group-fields mb-4">
                    <div className="label">{t('audience-segmentation.regions-label')}</div>
                    <Select
                        isMulti
                        options={regionOptions}
                        value={selectedRegions}
                        onChange={handleAudienceRegionsChange}
                        placeholder={t('audience-segmentation.regions-placeholder')}
                        noOptionsMessage={() => t('audience-segmentation.no-options')}
                    />
                </div>
            )}

            {schoolEnabled && (
                <div className="form-group-fields mb-4">
                    <div className="label">{t('audience-segmentation.schools-label')}</div>
                    <Select
                        isMulti
                        options={schoolOptions}
                        value={selectedAudienceSchools}
                        onChange={handleAudienceSchoolsChange}
                        placeholder={t('audience-segmentation.schools-placeholder')}
                        noOptionsMessage={() => t('audience-segmentation.no-options')}
                    />
                </div>
            )}

            {schoolEnabled && selectedAudienceSchools.length > 0 && (
                <div className="form-group-fields mb-4">
                    <div className="label">{t('audience-segmentation.specific-user-label')}</div>
                    <Select
                        isMulti
                        options={schoolUserOptions}
                        value={selectedSchoolUsers}
                        onChange={setSelectedSchoolUsers}
                        placeholder={t('audience-segmentation.school-users-placeholder')}
                        noOptionsMessage={() => t('audience-segmentation.no-options')}
                    />
                </div>
            )}

            {userEnabled && (
                <>
                    <div className="form-group-fields mb-4">
                        <div className="label">{t('audience-segmentation.filter-region-label')}</div>
                        <Select
                            options={regionOptions}
                            value={selectedUserFilterRegion}
                            onChange={handleUserFilterRegionChange}
                            placeholder={t('audience-segmentation.filter-region-placeholder')}
                            noOptionsMessage={() => t('audience-segmentation.no-options')}
                        />
                    </div>

                    {selectedUserFilterRegion && (
                        <div className="form-group-fields mb-4">
                            <div className="label">{t('audience-segmentation.filter-school-label')}</div>
                            <Select
                                isMulti
                                options={userFilterSchoolOptions}
                                value={selectedUserFilterSchools}
                                onChange={handleUserFilterSchoolsChange}
                                placeholder={t('audience-segmentation.filter-school-placeholder')}
                                noOptionsMessage={() => t('audience-segmentation.no-options')}
                            />
                        </div>
                    )}

                    {selectedUserFilterSchools.length > 0 && (
                        <div className="form-group-fields mb-4">
                            <div className="label">{t('audience-segmentation.filter-role-label')}</div>
                            <Select
                                options={roleOptionsWithAll}
                                value={selectedUserFilterRole}
                                onChange={setSelectedUserFilterRole}
                                placeholder={t('audience-segmentation.filter-role-placeholder')}
                                noOptionsMessage={() => t('audience-segmentation.no-options')}
                            />
                        </div>
                    )}

                    {selectedUserFilterRole && (
                        <div className="form-group-fields mb-4">
                            <div className="label">{t('audience-segmentation.specific-user-label')}</div>
                            <Select
                                isMulti
                                isDisabled={!hasUserFilter}
                                options={userOptions}
                                value={selectedUsers}
                                onChange={setSelectedUsers}
                                placeholder={hasUserFilter ? t('audience-segmentation.select-users-placeholder') : t('audience-segmentation.select-school-or-role-first-placeholder')}
                                noOptionsMessage={() => t('audience-segmentation.no-options')}
                            />
                            <p className="seg-hint" style={{ fontSize: '0.85em', color: '#9098AC', marginTop: '0.4em' }}>
                                {t('audience-segmentation.cascade-users-hint', { count: userOptions.length })}
                            </p>
                        </div>
                    )}
                </>
            )}

            {(selectedAudienceRoles.length > 0 ||
                selectedRegions.length > 0 ||
                selectedAudienceSchools.length > 0 ||
                selectedSchoolUsers.length > 0 ||
                selectedUserFilterRegion ||
                selectedUserFilterSchools.length > 0 ||
                selectedUserFilterRole ||
                selectedUsers.length > 0) && (
                <div className="form-group-fields mb-4">
                    <div className="label">{t('audience-segmentation.selected-audience-title')}</div>
                    <div>
                        {[
                            selectedAudienceRoles.length > 0 && {
                                heading: t('audience-segmentation.selected-roles-heading'),
                                values: selectedAudienceRoles.map(role => role.label).join(', '),
                            },
                            selectedRegions.length > 0 && {
                                heading: t('audience-segmentation.selected-regions-heading'),
                                values: selectedRegions.map(region => region.label).join(', '),
                            },
                            selectedAudienceSchools.length > 0 && {
                                heading: t('audience-segmentation.selected-schools-heading'),
                                values: selectedAudienceSchools.map(school => school.label).join(', '),
                            },
                            selectedSchoolUsers.length > 0 && {
                                heading: t('audience-segmentation.selected-school-users-heading'),
                                values: selectedSchoolUsers.map(user => user.label).join(', '),
                            },
                            (selectedUserFilterRegion || selectedUserFilterSchools.length > 0 || selectedUserFilterRole || selectedUsers.length > 0) && {
                                heading: t('audience-segmentation.selected-by-user-heading'),
                                values: [
                                    selectedUserFilterRegion?.label,
                                    ...selectedUserFilterSchools.map(school => school.label),
                                    selectedUserFilterRole?.label,
                                    selectedUsers.length > 0
                                        ? selectedUsers.map(user => user.label).join(', ')
                                        : (hasUserFilter ? t('audience-segmentation.all-matching-users', { count: userOptions.length }) : null),
                                ].filter(Boolean).join(' > '),
                            },
                        ]
                            .filter(Boolean)
                            .map((segment, index) => (
                                <React.Fragment key={segment.heading}>
                                    {index > 0 && ' · '}
                                    <strong>{segment.heading}</strong> {segment.values}
                                </React.Fragment>
                            ))}
                    </div>
                </div>
            )}
        </>
    );
};

export default AudienceSegmentation;
