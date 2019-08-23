Vue.component("person-data", {
    template: '#person-data',
    props: {
        person: null
    }
});

Vue.component("alert", {
    template: '#alert',
    props: {
        error: null
    },
    computed: {
        classObject: function () {
            return  "alert-" + this.error.level;
        }
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
        const self = this;
        navigator.mediaDevices.getUserMedia({video: true, audio: false})
            .then(function(stream) {
                this.video.srcObject = stream;
                this.video.play();
                self.$emit("removealert");
            })
            .catch(function(err) {
                self.$emit("showerror", err);
                console.log("An error occurred: " + err);
            });
    },
    methods: {
        capture() {
            const canvas = this.$refs.canvas;
            const context = canvas.getContext("2d");
            context.drawImage(this.video, 0, 0, canvas.width, canvas.height);
            this.taked = true;
            this.$emit("removealert");
        },
        upload(){
            const canvas = this.$refs.canvas;
            const img = canvas.toDataURL("image/png");
            const self = this;
            const config = { headers: {'Content-Type': 'application/json'} };
            axios.post(this.url, "\"" + img + "\"", config)
                .then(function (response) {
                    self.taked = false;
                    self.$emit("showinfo", "the capture has been uploaded");
                    console.log(response);
                })
                .catch(function (error) {
                    self.$emit("showerror", error);
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
        person: null,
        log: null
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
                    self.onRemoveAlert();
              })
              .catch(function (error) {
                    console.log(error);
                    self.onShowError("user not found");
              });
        },
        onShowError(msg){
            this.log = {
                level: "danger",
                msg: msg
            };
        },
        onShowInfo(msg){
            this.log = {
                level: "info",
                msg: msg
            };
        },
        onRemoveAlert(){
            this.log = null;
        },
        getAddFaceUrl(){
            if(this.person){
                return this.url + 'api/Person/' + this.person.id + '/facebydata'
            }
        }
    }
});