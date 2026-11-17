import React, { useState } from 'react';
import { withLocalize } from 'react-localize-redux';
import StoryList from './story-list';
import StoryEditor from './story-editor';

// Component that manages the story list and editor views
const StoriesView = () => {
    const [editing, setEditing] = useState(null);
    const [refreshKey, setRefreshKey] = useState(0);

    const openCreate = () => setEditing({ id: null });
    const openEdit = (id) => setEditing({ id });
    const backToList = () => setEditing(null);

    // Close the modal and refresh the list after saving a story
    const handleSaved = () => {
        setEditing(null);
        setRefreshKey((n) => n + 1);
    };

    return (
        <>
            <StoryList refreshKey={refreshKey} onCreate={openCreate} onEdit={openEdit} />

            {editing && (
                <StoryEditor
                    isOpen
                    storyId={editing.id}
                    onClose={backToList}
                    onSaved={handleSaved}
                />
            )}
        </>
    );
};

export default withLocalize(StoriesView);
