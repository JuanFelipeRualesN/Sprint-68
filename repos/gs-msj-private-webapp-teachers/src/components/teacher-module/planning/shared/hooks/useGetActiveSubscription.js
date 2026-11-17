import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetActiveSubscription = () => {
  const [subjectsAndGrades, setSubjectsAndGrades] = useState([]);
  const [academicPeriods, setAcademicPeriods] = useState([]);

  const getActiveSubscription = async () => {
    try {
      const endpoint = Settings.getSubscritionsAssociatedTeacherAPI();
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];

      // Flatten all SubscriptionProductModelList into one array
      const allProducts = data.flatMap(item => item.SubscriptionProductModelList || []);

      // Deduplicate by ProductId
      const uniqueProducts = Object.values(
          allProducts.reduce((acc, product) => {
            acc[product.ProductId] = product;
            return acc;
          }, {})
      );

      // Attach to state
      setSubjectsAndGrades(uniqueProducts);

      // ---- periods: flatten, keep future ones, dedupe, sort ----
      const now = new Date();

      const rawPeriods = data.flatMap(
          item =>
              item.EntityAcademicDetailFullResponseModel?.AcademicPeriodModelList?.filter(
                  p => new Date(p.PeriodTo) >= now
              ) || []
      );

      // Dedupe by Id; fallback to PeriodNumber+From+To
      const seen = new Set();
      const uniquePeriods = [];
      for (const p of rawPeriods) {
        const key = p.Id ?? `${p.PeriodNumber}|${p.PeriodFrom}|${p.PeriodTo}`;
        if (!seen.has(key)) {
          seen.add(key);
          uniquePeriods.push(p); // keeps the first occurrence
        }
      }

      // Optional: sort chronologically by start date
      uniquePeriods.sort((a, b) => new Date(a.PeriodFrom) - new Date(b.PeriodFrom));

      setAcademicPeriods(uniquePeriods);
    } catch (error) {}
  };

  return {
    getActiveSubscription,
    subjectsAndGrades,
    academicPeriods,
  };
};

export default useGetActiveSubscription;
