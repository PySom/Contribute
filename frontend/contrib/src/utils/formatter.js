export const formatAmountInNaira = (amount) => {
    if(amount == undefined){
        return null
    }
    return new Intl.NumberFormat("en-NG", { 
                    style: "currency", 
                    currency: "NGN" }).format(amount)
}

export const formatNameOrAnonymous = (name, isAnonymous) => {
    if(!isAnonymous){
        return name
    }
    if(name?.toLowerCase() === 'anonymous'){
        return 'Anonymous'
    }
    if(name && name.length >= 2){
        const firstLetter = name.charAt(0)
        const lastLetter = name.charAt(name.length - 1)
        const middleAsterisks = '*'.repeat(name.length - 2)
        return `${firstLetter}${middleAsterisks}${lastLetter}`
    }
    return '****';
}
