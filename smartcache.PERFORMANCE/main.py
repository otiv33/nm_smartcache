import requests
import concurrent.futures
import time
import matplotlib.pyplot as plt

def fetch_url(url, iter):
    elapsed_time = 0
    start_time = time.time()
    for i in range(5):
        requests.post(url+"test"+str(i)+"@email.com"+iter)
    for i in range(5):    
        requests.get(url+"test"+str(i)+"@email.com"+iter)
        requests.get(url+"test"+str(i)+"@email.com"+iter)
        requests.get(url+"test"+str(i)+"@email.com"+iter)
    elapsed_time = time.time() - start_time
    return elapsed_time

def make_request_test(url, num_of_requests=100):
    times = 0
    with concurrent.futures.ThreadPoolExecutor(max_workers=30) as executor:
        futures = [executor.submit(fetch_url, url, str(_)) for _ in range(num_of_requests)]
        for future in concurrent.futures.as_completed(futures):
            try:
                times += future.result()
            except Exception as e:
                print(f"An error occurred: {e}")
        return times

def compare_apis():
    smartcache_uri = "https://smartcacheapi-app-20231206183237.wittyocean-6dc4acf6.germanywestcentral.azurecontainerapps.io/emails/"
    traditional_uri = "https://traditionalapi-app-2023120709343.wittyocean-6dc4acf6.germanywestcentral.azurecontainerapps.io/emails/"
    num_of_requests = 10
    smartcache_times = make_request_test(smartcache_uri, num_of_requests)
    traditional_times = make_request_test(traditional_uri, num_of_requests)
    
    labels = ["Smart cache", "Traditional API with DB"]
    plt.bar(labels, [smartcache_times, traditional_times])
    plt.ylabel('Elapsed Time (seconds) for '+str(num_of_requests*20)+' requests')
    plt.title('HTTP Request Response Times')
    plt.show()
    print()
    
def smart_cache_performance():
    smartcache_uri = "https://smartcacheapi-app-20231206183237.wittyocean-6dc4acf6.germanywestcentral.azurecontainerapps.io/emails/"
    num_of_requests = 10
    print("Check if deployed only 1 instance")
    smartcache_times1 = make_request_test(smartcache_uri, num_of_requests)
    print("Deploy aditional instances - to 5")
    smartcache_times2 = make_request_test(smartcache_uri, num_of_requests)
    print("Deploy aditional instances - to 10")
    smartcache_times3 = make_request_test(smartcache_uri, num_of_requests)
    
    labels = ["1 instance", "5 instances", "10 instances"]
    plt.bar(labels, [smartcache_times1, smartcache_times2, smartcache_times3])
    plt.ylabel('Elapsed Time (seconds) for '+str(num_of_requests*20)+' requests')
    plt.title('HTTP Request Response Times for Smart cache')
    plt.show()
    print()
    

if __name__ == "__main__":
    smart_cache_performance()