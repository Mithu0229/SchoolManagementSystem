import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-notfound',
  standalone: true,
  imports: [RouterModule, ButtonModule],
  template: ` <div
    class="flex items-center justify-center min-h-screen overflow-hidden"
  >
    <div class="flex flex-col items-center justify-center">
      <!-- <div class="w-14 h-14 my-2">
        <svg
          width="56"
          height="56"
          viewBox="0 0 56 56"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          class="w-full h-full"
        >
          <circle
            cx="28"
            cy="28"
            r="27"
            fill="white"
            stroke="#363763"
            stroke-width="2"
          />
          <circle cx="28" cy="28" r="24" fill="#363763" />
          <path
            d="M13.457 27.939C13.457 28.4943 13.5317 28.9937 13.681 29.437C13.8303 29.8757 14.0427 30.249 14.318 30.557C14.598 30.8603 14.934 31.0937 15.326 31.257C15.7227 31.4203 16.1683 31.502 16.663 31.502C17.1577 31.502 17.601 31.4203 17.993 31.257C18.3897 31.0937 18.7257 30.8603 19.001 30.557C19.2763 30.249 19.4887 29.8757 19.638 29.437C19.7873 28.9937 19.862 28.4943 19.862 27.939C19.862 27.3837 19.7873 26.8867 19.638 26.448C19.4887 26.0047 19.2763 25.629 19.001 25.321C18.7257 25.013 18.3897 24.7773 17.993 24.614C17.601 24.4507 17.1577 24.369 16.663 24.369C16.1683 24.369 15.7227 24.4507 15.326 24.614C14.934 24.7773 14.598 25.013 14.318 25.321C14.0427 25.629 13.8303 26.0047 13.681 26.448C13.5317 26.8867 13.457 27.3837 13.457 27.939ZM22.312 34.981H20.758C20.534 34.981 20.331 34.9507 20.149 34.89C19.9717 34.8293 19.8083 34.7173 19.659 34.554L18.175 32.916C17.937 32.9813 17.692 33.0303 17.44 33.063C17.1927 33.0957 16.9337 33.112 16.663 33.112C15.8977 33.112 15.2 32.9837 14.57 32.727C13.94 32.4657 13.3987 32.104 12.946 31.642C12.498 31.18 12.1503 30.634 11.903 30.004C11.6557 29.3693 11.532 28.681 11.532 27.939C11.532 27.197 11.6557 26.511 11.903 25.881C12.1503 25.2463 12.498 24.698 12.946 24.236C13.3987 23.774 13.94 23.4147 14.57 23.158C15.2 22.8967 15.8977 22.766 16.663 22.766C17.1763 22.766 17.6593 22.8267 18.112 22.948C18.5647 23.0647 18.98 23.2327 19.358 23.452C19.736 23.6667 20.0743 23.9303 20.373 24.243C20.6763 24.551 20.933 24.8963 21.143 25.279C21.353 25.6617 21.5117 26.077 21.619 26.525C21.731 26.973 21.787 27.4443 21.787 27.939C21.787 28.3917 21.7403 28.8257 21.647 29.241C21.5537 29.6517 21.4183 30.0367 21.241 30.396C21.0683 30.7553 20.856 31.0867 20.604 31.39C20.352 31.6887 20.065 31.9523 19.743 32.181L22.312 34.981ZM34.4096 22.878V33H32.7506V26.462C32.7506 26.2007 32.7646 25.9183 32.7926 25.615L29.7336 31.362C29.589 31.6373 29.3673 31.775 29.0686 31.775H28.8026C28.504 31.775 28.2823 31.6373 28.1376 31.362L25.0436 25.594C25.0576 25.748 25.0693 25.8997 25.0786 26.049C25.088 26.1983 25.0926 26.336 25.0926 26.462V33H23.4336V22.878H24.8546C24.9386 22.878 25.011 22.8803 25.0716 22.885C25.1323 22.8897 25.186 22.9013 25.2326 22.92C25.284 22.9387 25.3283 22.969 25.3656 23.011C25.4076 23.053 25.4473 23.109 25.4846 23.179L28.5156 28.8C28.595 28.9493 28.6673 29.1033 28.7326 29.262C28.8026 29.4207 28.8703 29.584 28.9356 29.752C29.001 29.5793 29.0686 29.4137 29.1386 29.255C29.2086 29.0917 29.2833 28.9353 29.3626 28.786L32.3516 23.179C32.389 23.109 32.4286 23.053 32.4706 23.011C32.5126 22.969 32.557 22.9387 32.6036 22.92C32.655 22.9013 32.711 22.8897 32.7716 22.885C32.8323 22.8803 32.9046 22.878 32.9886 22.878H34.4096ZM42.058 24.719C42.002 24.817 41.9413 24.8893 41.876 24.936C41.8153 24.978 41.7383 24.999 41.645 24.999C41.547 24.999 41.4397 24.964 41.323 24.894C41.211 24.8193 41.0757 24.7377 40.917 24.649C40.7583 24.5603 40.5717 24.481 40.357 24.411C40.147 24.3363 39.8973 24.299 39.608 24.299C39.3467 24.299 39.118 24.3317 38.922 24.397C38.726 24.4577 38.5603 24.544 38.425 24.656C38.2943 24.768 38.1963 24.9033 38.131 25.062C38.0657 25.216 38.033 25.3863 38.033 25.573C38.033 25.811 38.0983 26.0093 38.229 26.168C38.3643 26.3267 38.5417 26.462 38.761 26.574C38.9803 26.686 39.23 26.7863 39.51 26.875C39.79 26.9637 40.077 27.0593 40.371 27.162C40.665 27.26 40.952 27.3767 41.232 27.512C41.512 27.6427 41.7617 27.8107 41.981 28.016C42.2003 28.2167 42.3753 28.464 42.506 28.758C42.6413 29.052 42.709 29.409 42.709 29.829C42.709 30.2863 42.6297 30.7157 42.471 31.117C42.317 31.5137 42.0883 31.8613 41.785 32.16C41.4863 32.454 41.12 32.6873 40.686 32.86C40.252 33.028 39.755 33.112 39.195 33.112C38.873 33.112 38.5557 33.0793 38.243 33.014C37.9303 32.9533 37.6293 32.8647 37.34 32.748C37.0553 32.6313 36.787 32.4913 36.535 32.328C36.283 32.1647 36.059 31.9827 35.863 31.782L36.416 30.879C36.4627 30.8137 36.5233 30.76 36.598 30.718C36.6727 30.6713 36.752 30.648 36.836 30.648C36.9527 30.648 37.0787 30.697 37.214 30.795C37.3493 30.8883 37.5103 30.9933 37.697 31.11C37.8837 31.2267 38.1007 31.334 38.348 31.432C38.6 31.5253 38.901 31.572 39.251 31.572C39.7877 31.572 40.203 31.446 40.497 31.194C40.791 30.9373 40.938 30.571 40.938 30.095C40.938 29.829 40.8703 29.612 40.735 29.444C40.6043 29.276 40.4293 29.136 40.21 29.024C39.9907 28.9073 39.741 28.8093 39.461 28.73C39.181 28.6507 38.8963 28.5643 38.607 28.471C38.3177 28.3777 38.033 28.2657 37.753 28.135C37.473 28.0043 37.2233 27.834 37.004 27.624C36.7847 27.414 36.6073 27.1527 36.472 26.84C36.3413 26.5227 36.276 26.133 36.276 25.671C36.276 25.3023 36.3483 24.943 36.493 24.593C36.6423 24.243 36.857 23.9327 37.137 23.662C37.417 23.3913 37.7623 23.1743 38.173 23.011C38.5837 22.8477 39.055 22.766 39.587 22.766C40.1843 22.766 40.735 22.8593 41.239 23.046C41.743 23.2327 42.1723 23.494 42.527 23.83L42.058 24.719Z"
            fill="white"
          />
        </svg>
      </div> -->
      <div
        style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, color-mix(in srgb, var(--primary-color), transparent 60%) 10%, var(--surface-ground) 30%)"
      >
        <div
          class="w-full bg-surface-0 dark:bg-surface-900 py-10 px-8 sm:px-10 flex flex-col items-center"
          style="border-radius: 53px"
        >
          <span class="text-primary font-bold text-3xl">404</span>
          <h1
            class="text-surface-900 dark:text-surface-0 font-bold text-3xl lg:text-5xl mb-2"
          >
            Not Found
          </h1>
          <div class="text-surface-600 dark:text-surface-200 mb-8">
            Requested resource is not available.
          </div>
          <a
            routerLink="/"
            class="w-full flex items-center py-8 border-surface-300 dark:border-surface-500 border-b"
          >
            <span
              class="flex justify-center items-center border-2 border-primary text-primary rounded-border"
              style="height: 3.5rem; width: 3.5rem"
            >
              <i class="pi pi-fw pi-table !text-2xl"></i>
            </span>
            <span class="ml-6 flex flex-col">
              <span
                class="text-surface-900 dark:text-surface-0 lg:text-xl font-medium mb-0 block"
                >Frequently Asked Questions</span
              >
              <span class="text-surface-600 dark:text-surface-200 lg:text-xl"
                >Ultricies mi quis hendrerit dolor.</span
              >
            </span>
          </a>
          <a
            routerLink="/"
            class="w-full flex items-center py-8 border-surface-300 dark:border-surface-500 border-b"
          >
            <span
              class="flex justify-center items-center border-2 border-primary text-primary rounded-border"
              style="height: 3.5rem; width: 3.5rem"
            >
              <i class="pi pi-fw pi-question-circle !text-2xl"></i>
            </span>
            <span class="ml-6 flex flex-col">
              <span
                class="text-surface-900 dark:text-surface-0 lg:text-xl font-medium mb-0"
                >Solution Center</span
              >
              <span class="text-surface-600 dark:text-surface-200 lg:text-xl"
                >Phasellus faucibus scelerisque eleifend.</span
              >
            </span>
          </a>
          <a
            routerLink="/"
            class="w-full flex items-center mb-8 py-8 border-surface-300 dark:border-surface-500 border-b"
          >
            <span
              class="flex justify-center items-center border-2 border-primary text-primary rounded-border"
              style="height: 3.5rem; width: 3.5rem"
            >
              <i class="pi pi-fw pi-unlock !text-2xl"></i>
            </span>
            <span class="ml-6 flex flex-col">
              <span
                class="text-surface-900 dark:text-surface-0 lg:text-xl font-medium mb-0"
                >Permission Manager</span
              >
              <span class="text-surface-600 dark:text-surface-200 lg:text-xl"
                >Accumsan in nisl nisi scelerisque</span
              >
            </span>
          </a>
          <p-button label="Go to Dashboard" routerLink="/" />
        </div>
      </div>
    </div>
  </div>`,
})
export class Notfound {}
