import { useEffect, useState } from "react"

import useServerAPI from "./useServerAPI"

const useForm = (initialContributions) => {
    const [name, setName] = useState('')
  const [amount, setAmount] = useState('')
  const [anonymous, setAnonymous] = useState(false)
  const [contributions, setContributions] = useState(initialContributions ?? [])
  const [amountError, setAmountError] = useState('')
  const [nameError, setNameError] = useState('')

  const { postContributor } = useServerAPI();
  

  useEffect(() => {
    setContributions(initialContributions ?? [])
  }, [initialContributions])

  function validateAmountInput(val) {
    const raw = (val || '').trim().replace(/,/g, '.')
    if (raw === '') return { valid: false, error: 'Enter an amount' }

    const num = parseFloat(raw)
    if (!(num > 0)) return { valid: false, error: 'Amount must be greater than 0' }
    return { valid: true, value: num }
  }

  function handleSubmit(e, id) {
    e.preventDefault()
    const res = validateAmountInput(amount)
    if (!res.valid) {
      setAmountError(res.error)
      return
    }

    const numericAmount = res.value

    const reference = crypto.randomUUID();

    const entry = {
      name: name.trim() || 'Anonymous',
      amount: numericAmount,
      isAnonymous: anonymous,
      reference,
      receipientId: id,
    }

    const onSuccess = async () => {
      await postContributor(entry);
      setContributions((c) => [entry, ...c])
      setName('')
      setAmount('')
      setAnonymous(false)
      setAmountError('')
    }



    const options = {
        amount: numericAmount,
        currency: 'NGN',
        domain: 'live',
        key: 'ec26a818-2105-4aea-b94a-8122aeaf95c5',
        email: '',
        transactionref: reference,
        customer_logo:'',
        customer_service_channel: '+2348038714611',
        txn_charge: 1.5,
        txn_charge_type: 'percentage',
        onSuccess,
        onExit: function(response) { console.log('Hello World!', response.message); }
    }
                
    if(window.VPayDropin){
        const {open} = window.VPayDropin.create(options);
        open();                    
    }   
    
  }

  const onAnonymousChange = (isAnon) => {
    setAnonymous(isAnon)
    if (isAnon) {
      setNameError('')
    }
  }

  function handleAmountChange(e) {
    const val = e.target.value
    // keep raw user input so they can type freely (we'll validate/format on blur/submit)
    setAmount(val)
    if (amountError) setAmountError('')
  }

    function handleNameChange(e) {
    const val = e.target.value
    // keep raw user input so they can type freely (we'll validate/format on blur/submit)
    setName(val)
    if (nameError) setNameError('')
  }

  function handleNameBlur() {
    const isValid = name.trim().length > 0 || anonymous
    if (!isValid) {
      setNameError('Please enter your name or mark as anonymous.')
      return
    }
    setNameError('')
  }

  function handleAmountBlur() {
    const res = validateAmountInput(amount)
    if (!res.valid) {
      setAmountError(res.error)
      return
    }
    // format to two decimals for clarity
    setAmount(res.value.toFixed(2))
    setAmountError('')
  }

  const isAmountValid = validateAmountInput(amount).valid
  const isNameValid = name.trim().length > 0 || anonymous;

  return {
    name,
    amount,
    setAmount,
    anonymous,
    onAnonymousChange,
    contributions,
    amountError,
    isAmountValid,
    isNameValid,
    handleSubmit,
    handleAmountChange,
    handleAmountBlur,
    handleNameChange,
    handleNameBlur,
    nameError,
  }
}
export default useForm;