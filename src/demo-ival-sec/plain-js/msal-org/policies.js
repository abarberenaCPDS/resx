/**
 * Enter here the user flows and custom policies for your B2C application
 * To learn more about user flows, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
const b2cPolicies = {
    names: {
        signUpSignIn: "B2C_1A_SIGNUP_SIGNIN",
        // editProfile: "B2C_1_edit_profile_v2"
    },
    authorities: {
        signUpSignIn: {
            authority: "https://iamvresdnadev001.b2clogin.com/iamvresdnadev001.onmicrosoft.com/B2C_1A_SIGNUP_SIGNIN",
            // authority: "https://fabrikamb2c.b2clogin.com/fabrikamb2c.onmicrosoft.com/B2C_1_susi_reset_v2",
        },
        // editProfile: {
        //     authority: "https://fabrikamb2c.b2clogin.com/fabrikamb2c.onmicrosoft.com/B2C_1_edit_profile_v2"
        // }
    },
    authorityDomain: "iamvresdnadev001.b2clogin.com"
    // authorityDomain: "fabrikamb2c.b2clogin.com"
}

const b2cPoliciesFabrikam = {
    names: {
        signUpSignIn: "B2C_1_susi_reset_v2",
        editProfile: "B2C_1_edit_profile_v2"
    },
    authorities: {
        signUpSignIn: {
            authority: "https://fabrikamb2c.b2clogin.com/fabrikamb2c.onmicrosoft.com/B2C_1_susi_reset_v2",
        },
        editProfile: {
            authority: "https://fabrikamb2c.b2clogin.com/fabrikamb2c.onmicrosoft.com/B2C_1_edit_profile_v2"
        }
    },
    authorityDomain: "fabrikamb2c.b2clogin.com"
}