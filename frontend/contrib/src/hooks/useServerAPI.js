import { useCallback, useState } from "react";

import api from "../utils/api";

const useServerAPI = () => {
  const [error, setError] = useState({
    receipientsError: null, 
    receipientBySlugError: null, 
    contributorsPostError: null
  });

  const getReceipients = useCallback(async () => {
    setError((prev) => ({...prev, receipientsError: null}));
    const result = await api.get('receipients');
    if(result === null){
      setError((prev) => ({...prev, receipientsError: 'Failed to fetch receipients'}));
    }
    return result;
  }, [])

  const getReceipientBySlug = useCallback(async (slug) => {
    setError((prev) => ({...prev, receipientBySlugError: null}));
    const result = await api.get(`receipients/${slug}`);
    if(result === null){
      setError((prev) => ({...prev, receipientBySlugError: 'Failed to fetch receipient'}));
    }
    return result;
  }, [])

  const postContributor = useCallback(async (contributor) => {
    setError((prev) => ({...prev, contributorsPostError: null}));
    const result = await api.post('contributors', contributor);
    if(result === null){
      setError((prev) => ({...prev, contributorsPostError: 'Failed to post contributor'}));
    }
    return result;
  }, [])

  return {
    postContributor,
    getReceipients,
    getReceipientBySlug,
    error,
  };
}

export default useServerAPI;