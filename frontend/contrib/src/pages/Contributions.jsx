import { useEffect, useState } from 'react';

import Contributors from '../components/Contributors'
import Form from '../components/Form'
import { formatAmountInNaira } from '../utils/formatter';
import useForm from '../hooks/useForm'
import { useParams } from 'react-router-dom';
import useServerAPI from '../hooks/useServerAPI';

function Contributions() {

  const { slug } = useParams();

  const { getReceipientBySlug, error: { receipientBySlugError } } = useServerAPI();
  const [contributors, setContributors] = useState([]);
  const [isFetching, setIsFetching] = useState(false);
  const [currentReceipient, setCurrentReceipient] = useState(null);

  useEffect(() => {
    const fetchReceipient = async () => {
      if (slug) {
        setIsFetching(true);
        const {receipient, contributors} = await getReceipientBySlug(slug) || {};
        setIsFetching(false);
        if (receipient) {
          document.title = `Contribute for ${receipient.name} | IMT 2016 Contributions`;
        }
        setCurrentReceipient(receipient);
        setContributors(contributors || []);
      }
    };
    fetchReceipient();
  }, [slug, getReceipientBySlug]);

    const {
    name,
    amount,
    anonymous,
    contributions,
    amountError,
    isAmountValid,
    handleSubmit,
    handleAmountChange,
    handleAmountBlur,
    handleNameChange,
    handleNameBlur,
    nameError,
    isNameValid,
    onAnonymousChange,
  } = useForm(contributors)

  if(isFetching){
    return <p>Loading...</p>
  }
  
  if(receipientBySlugError){
    return <p>Error loading receipient</p>
  }

  const totalContributions = contributions.reduce((sum, c) => sum + c.amount, 0); 

  

  return (
    <>
      <h3>IMT 2016 Contribution</h3>
      <div className="content-grid">
        {currentReceipient ? (
          <>

            <div className="left">
            <h4>Contributing for {currentReceipient.name} (Total <strong>{formatAmountInNaira(totalContributions)}</strong>)</h4>
            <Form 
                amount={amount} 
                amountError={amountError} 
                handleSubmit={(e) => handleSubmit(e, currentReceipient.id)} 
                isAmountValid={isAmountValid} 
                isAnonymous={anonymous} 
                onAmountBlur={handleAmountBlur} 
                onAmountChange={handleAmountChange} 
                onAnonymousChange={onAnonymousChange} 
                onNameChange={handleNameChange}
                onNameBlur={handleNameBlur}
                nameError={nameError} 
                userName={name}
                isNameValid={isNameValid}
                />
            </div>

            <aside className="right contrib-list" aria-labelledby="recent-heading">
            <Contributors contributions={contributions} />
            </aside>
        </>)
        : (<p>No contributions at this time. Please check later.</p>
        )}
      </div>
    </>
    
  )
}

export default Contributions