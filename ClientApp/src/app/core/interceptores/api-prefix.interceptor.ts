import { HttpInterceptorFn } from "@angular/common/http";
import { environment } from "../../../environments/environment";

export const apiPrefixInterceptor: HttpInterceptorFn = (req, next) => {
    const apiUrl = environment.apiUrl;
    const apiReq = req.clone({ url: `${apiUrl}${req.url}` });
    return next(apiReq);
};
