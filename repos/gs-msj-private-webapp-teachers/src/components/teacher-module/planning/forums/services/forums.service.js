import axios from "../../../../../api/interceptors";
import moment from "moment";
import Settings from "../../../../../custom-classes/settings";

const getForums = async (planningId, limit, pageNumber) => {
  try {
    const endpoint = Settings.getForumsTeacher(planningId, pageNumber, limit, null, null);
    const response = await axios.get(endpoint);
    return response.status === 200 ? response.data : [];
  } catch (error) {
    return Promise.resolve([]);
  }
};

const deleteForum = async (forumId) => {
  const endpoint = Settings.getForumArchiveAPI(forumId);
  const response = await axios.delete(endpoint);
  return Promise.resolve(response.status === 200);
};

const createForum = async (forum) => {
  const data = {
    TeacherPlanId: forum.planningId,
    Title: forum.title,
    Description: forum.description,
    ImplementationDates: forum.implementationDates?.map((d) => ({
      ImplementationDate: moment(d).format("YYYY/MM/DD"),
    })),
  };

  const endpoint = Settings.getForumCreationAPI();
  const response = await axios.post(endpoint, data);
  const result = response.status === 201;

  return Promise.resolve(result);
};

const updateForum = async (forum) => {
  const data = {
    Id: forum.id,
    TeacherPlanId: forum.planningId,
    Title: forum.title,
    Description: forum.description,
    ImplementationDates: forum.implementationDates?.map((d) => ({
      ImplementationDate: moment(d).format("YYYY/MM/DD"),
    })),
  };

  const endpoint = Settings.getForumModifyAPI(forum.id);
  const response = await axios.put(endpoint, data);
  const result = response.status === 200;

  return Promise.resolve(result);
};

export { getForums, deleteForum, createForum, updateForum };
