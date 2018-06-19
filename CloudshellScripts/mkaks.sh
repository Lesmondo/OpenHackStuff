#!/bin/bash

#export RG=minecraft-aks-les
#export NAME=minecraft-aks-les

export RG=minecraft-ss2
export NAME=minecraft-ss-AKSCluster


#az group create -g $RG -l eastus
#az aks create --resource-group $RG --name $NAME --node-count 1 --generate-ssh-keys
az aks get-credentials --resource-group $RG --name $NAME


#kubectl apply -f minecraft.yaml
kubectl get services
