import { useEffect, useState } from "react";

import { Link } from "react-router-dom";
import useServerAPI from "../hooks/useServerAPI";

const Home = () => {
    const { getReceipients, error: { receipientsError } } = useServerAPI();
    const [receipients, setReceipients] = useState([]);
  const [isFetching, setIsFetching] = useState(false);
    useEffect(() => {
        const fetchReceipients = async () => {
            setIsFetching(true);
            const data = await getReceipients();
            setIsFetching(false);
            setReceipients(data);
        };
        fetchReceipients();
    }, [getReceipients]);
    if(isFetching){
        return <p>Loading...</p>
    }

    if(receipientsError){
        return <p>Error loading receipients</p>
    }
    return (
        <div>
            <h2>Welcome to the IMT Contributions Platform</h2>
            <p>
                This platform allows you to contribute to various causes and projects associated with IMT 2016. Browse through the available contributions and make your donation today!
            </p>
            <h3>Available Receipients</h3>
            {receipients.length === 0 ? (
                <p>No receipients available at the moment.</p>
            ) : (
                <ul>
                    {receipients.map((r) => (
                        <li key={r.slug}>
                            <Link className="receipient-link" to={`/contributions/${r.slug}`}>{r.name}</Link>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
    
}

export default Home