Vue.component("person-data", {
    template: '#person-data',
    props: {
        person: null
    }
});

Vue.component("capture", {
    template: '#capture',
    props: ['url'],
    data() {
        return {
            video: {},
            taked: false
        }
    },
    mounted: function () {
        this.video = this.$refs.video;
        navigator.mediaDevices.getUserMedia({video: true, audio: false})
            .then(function(stream) {
                this.video.srcObject = stream;
                this.video.play();
            })
            .catch(function(err) {
                console.log("An error occurred: " + err);
            });
    },
    methods: {
        capture() {
            const canvas = this.$refs.canvas;
            const context = canvas.getContext("2d");
            context.drawImage(this.video, 0, 0, canvas.width, canvas.height);
            this.taked = true;
        },
        upload(){
            const canvas = this.$refs.canvas;
            const img = canvas.toDataURL("image/png");

            const config = { headers: {'Content-Type': 'application/json'} };
            axios.post(this.url, "\"" + img + "\"", config)
                .then(function (response) {
                    console.log(response);
                })
                .catch(function (error) {
                    console.log(error);
                });
        }
    }
});

const router = new VueRouter({
    routes: [
        { path: '/:personid' }
    ]
  })

var app = new Vue({
    router,
    el: '#app',
    props: {
        url: {
            type: String,
            default: 'https://localhost:44377/'
        }
    },
    data: {
        person: null
    },
    created: function () {
        const id = this.$route.params.personid
        this.getPerson(id);
    },
    methods: {
        getPerson(personid){
            const self = this;
            const uri = this.url + 'api/Person/' + personid;
            axios.get(uri)
                .then(function (response) {
                    self.person = response.data;
              })
              .catch(function (error) {
                    console.log(error);
              });
        },
        getAddFaceUrl(){
            if(this.person){
                return this.url + 'api/Person/' + this.person.id + '/facebydata'
            }
        }
    }
});