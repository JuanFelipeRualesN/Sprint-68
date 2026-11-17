import axios from "./interceptors";
import Settings from "../custom-classes/settings";

export const createForum = (teacherPlanId, forumTitle, forumDescription) =>{
    let request = {
        TeacherPlanId: teacherPlanId,
        Title: forumTitle,
        Description: forumDescription,
    };

    return axios({
        method: "POST",
        url: `${Settings.API_TEACHER_URL}/teachersapi/forums/`,
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

export const updateForum = (forumId, forumTitle, forumDescription) =>{
    let request = {
        Active: "1",
        Id: forumId,
        Title: forumTitle,
        Description: forumDescription,
    };

    return axios({
        method: "PUT",
        url: `${Settings.API_TEACHER_URL}/teachersapi/forums/${forumId}`,
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

export const getForumDetails = (forumId, page, pageSize, postId) => {
    if(typeof page === 'undefined' || page === null) {
        page = 1;
    }

    if(typeof pageSize === 'undefined' || pageSize === null) {
        pageSize = 10;
    }

    page = page - 1;

    let url = `${Settings.API_TEACHER_URL}/teachersapi/forums/${forumId}?fromPage=${page}&limiter=${pageSize}`;
    if (postId) {
        url += `&forumParentId=${postId}`;
    }
    return axios.get(
        url
    );
};

export const createPost = (data) => {
    return axios({
        method: "POST",
        url: `${Settings.API_TEACHER_URL}/teachersapi/forums/${data.ForumId}/post/`,
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

export const updatePost = (data) => {
    return axios({
        method: "PUT",
        url: `${Settings.API_TEACHER_URL}/teachersapi/forums/${data.ForumId}/post/${data.Id}`,
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

export const deletePost = (forumId, postId) => {
    return axios({
        method: "DELETE",
        url: `${Settings.API_TEACHER_URL}/teachersapi/forums/${forumId}/post/${postId}`
    });
};
