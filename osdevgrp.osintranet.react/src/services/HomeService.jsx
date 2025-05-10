import ServiceBase from "./ServiceBase";

export default class HomeService extends ServiceBase {
    async getLayoutContext() {
        const response = await fetch(this.resolveEndpoint('/api/home/index'));
        if (response.ok) {
            return await response.json();
        }

        throw new Error(response.statusText);
    }
}