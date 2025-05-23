﻿/* Copyright 2019-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver.Encryption
{
    /// <summary>
    /// Client encryption options.
    /// </summary>
    public sealed class ClientEncryptionOptions
    {
        // private fields
        private TimeSpan? _keyExpiration;
        private readonly IMongoClient _keyVaultClient;
        private readonly CollectionNamespace _keyVaultNamespace;
        private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> _kmsProviders;
        private readonly IReadOnlyDictionary<string, SslSettings> _tlsOptions;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientEncryptionOptions"/> class.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="keyVaultNamespace">The key vault namespace.</param>
        /// <param name="kmsProviders">The KMS providers.</param>
        /// <param name="tlsOptions">The tls options.</param>
        public ClientEncryptionOptions(
            IMongoClient keyVaultClient,
            CollectionNamespace keyVaultNamespace,
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> kmsProviders,
            Optional<IReadOnlyDictionary<string, SslSettings>> tlsOptions = default)
            : this(keyVaultClient, keyVaultNamespace, kmsProviders, tlsOptions, keyExpiration: null)
        {
        }

        private ClientEncryptionOptions(
            IMongoClient keyVaultClient,
            CollectionNamespace keyVaultNamespace,
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> kmsProviders,
            Optional<IReadOnlyDictionary<string, SslSettings>> tlsOptions = default,
            Optional<TimeSpan?> keyExpiration = default)
        {
            _keyVaultClient = Ensure.IsNotNull(keyVaultClient, nameof(keyVaultClient));
            _keyVaultNamespace = Ensure.IsNotNull(keyVaultNamespace, nameof(keyVaultNamespace));
            _kmsProviders = Ensure.IsNotNull(kmsProviders, nameof(kmsProviders));
            _tlsOptions = tlsOptions.WithDefault(new Dictionary<string, SslSettings>());
            _keyExpiration = keyExpiration.WithDefault(null);

            EnsureKmsProvidersAreValid(_kmsProviders);
            EnsureKmsProvidersTlsSettingsAreValid(_tlsOptions);
        }

        // public properties

        /// <summary>
        /// Gets the data encryption key cache expiration time.
        /// </summary>
        public TimeSpan? KeyExpiration => _keyExpiration;

        /// <summary>
        /// Gets the key vault client.
        /// </summary>
        /// <value>
        /// The key vault client.
        /// </value>
        public IMongoClient KeyVaultClient => _keyVaultClient;

        /// <summary>
        /// Gets the key vault namespace.
        /// </summary>
        /// <value>
        /// The key vault namespace.
        /// </value>
        public CollectionNamespace KeyVaultNamespace => _keyVaultNamespace;

        /// <summary>
        /// Gets the KMS providers.
        /// </summary>
        /// <value>
        /// The KMS providers.
        /// </value>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> KmsProviders => _kmsProviders;

        /// <summary>
        /// Gets the tls options.
        /// </summary>
        /// <value>
        /// The tls options.
        /// </value>
        public IReadOnlyDictionary<string, SslSettings> TlsOptions => _tlsOptions;

        /// <summary>
        /// Returns a new ClientEncryptionOptions instance with some settings changed.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="keyVaultNamespace">The key vault namespace.</param>
        /// <param name="kmsProviders">The KMS providers.</param>
        /// <param name="tlsOptions">The tls options.</param>
        /// <returns>A new ClientEncryptionOptions instance.</returns>
        public ClientEncryptionOptions With(
            Optional<IMongoClient> keyVaultClient = default,
            Optional<CollectionNamespace> keyVaultNamespace = default,
            Optional<IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>> kmsProviders = default,
            Optional<IReadOnlyDictionary<string, SslSettings>> tlsOptions = default)
        {
            return new ClientEncryptionOptions(
                keyVaultClient: keyVaultClient.WithDefault(_keyVaultClient),
                keyVaultNamespace: keyVaultNamespace.WithDefault(_keyVaultNamespace),
                kmsProviders: kmsProviders.WithDefault(_kmsProviders),
                tlsOptions: Optional.Create(tlsOptions.WithDefault(_tlsOptions)),
                keyExpiration: _keyExpiration);
        }

        /// <summary>
        /// Sets the data encryption key cache expiration time. If not set, it defaults to 60 seconds.
        /// If set to TimeSpan.Zero, the cache never expires.
        /// </summary>
        /// <param name="keyExpiration">The data encryption key cache expiration time.</param>
        public void SetKeyExpiration(TimeSpan? keyExpiration)
        {
            _keyExpiration = keyExpiration;
        }

        private static void EnsureKmsProvidersAreValid(IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> kmsProviders)
        {
            foreach (var kmsProvider in kmsProviders)
            {
                foreach (var option in Ensure.IsNotNull(kmsProvider.Value, nameof(kmsProvider)))
                {
                    var optionValue = Ensure.IsNotNull(option.Value, "kmsProviderOption");
                    var isValid = optionValue is byte[] || optionValue is string;
                    if (!isValid)
                    {
                        throw new ArgumentException($"Invalid kms provider option type: {optionValue.GetType().Name}.");
                    }
                }
            }
        }

        private static void EnsureKmsProvidersTlsSettingsAreValid(IReadOnlyDictionary<string, SslSettings> kmsProviderTlsSettings)
        {
            if (kmsProviderTlsSettings == null)
            {
                return;
            }

            foreach (var kmsProviderTlsSetting in kmsProviderTlsSettings)
            {
                var kmsProviderTlsSettingValue = Ensure.IsNotNull(kmsProviderTlsSetting.Value, nameof(kmsProviderTlsSetting.Value));

                if (kmsProviderTlsSettingValue.ServerCertificateValidationCallback != null) // tlsInsecure
                {
                    throw new ArgumentException("Insecure TLS options prohibited.");
                }
            }
        }
    }
}
