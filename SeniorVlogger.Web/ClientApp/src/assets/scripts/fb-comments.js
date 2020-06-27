let register = (d = document, s = 'script', id = 'facebook-jssdk') => {
    var js, fjs = d.getElementsByTagName(s)[0]
    if (d.getElementById(id)) {return}
    js = d.createElement(s)
    js.id = id
    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v6.0"
    fjs.parentNode.insertBefore(js, fjs)
}


window.fbAsyncInit = function() {
    FB.init({
        appId: '633316797494891',
        xfbml: true,
        version: 'v6.0',
        autoLogAppEvents: true
    })

    window.dispatchEvent(new Event('fb-sdk-ready'))
}

let VueFacebook = {
    install(Vue, options) {
        Vue.mixin({
            created() {
                register()
            }
        })
    }
}

export default VueFacebook;